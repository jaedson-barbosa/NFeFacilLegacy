using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NFeFacil.Sincronizacao.FastServer
{
    class RestControllerRequestHandler
    {
        ImmutableArray<RestControllerMethodInfo> _restMethodCollection;
        Dictionary<Type, object> ObjectCache = new Dictionary<Type, object>();

        internal RestControllerRequestHandler()
        {
            _restMethodCollection = ImmutableArray<RestControllerMethodInfo>.Empty;
        }

        internal void RegisterController<T>() where T : class, new()
        {
            var restControllerMethodInfos = GetRestMethods<T>();
            AddRestMethods<T>(restControllerMethodInfos);
        }

        void AddRestMethods<T>(IEnumerable<RestControllerMethodInfo> restControllerMethodInfos) where T : class, new()
        {
            _restMethodCollection = _restMethodCollection.Concat(restControllerMethodInfos)
                .OrderByDescending(x => x.MethodInfo.GetParameters().Count())
                .ToImmutableArray();

            ObjectCache.Add(typeof(T), new T());
        }

        internal IEnumerable<RestControllerMethodInfo> GetRestMethods<T>() where T : class
        {
            foreach (var restMethod in typeof(T).GetRuntimeMethods())
            {
                if (restMethod.IsPublic && restMethod.IsDefined(typeof(UriFormatAttribute)))
                if (HasRestResponse(restMethod))
                    yield return new RestControllerMethodInfo(restMethod, false);
                else if (HasAsyncRestResponse(restMethod))
                    yield return new RestControllerMethodInfo(restMethod, true);
            }
        }

        private static bool HasRestResponse(MethodInfo m)
        {
            return m.ReturnType == typeof(RestResponse);
        }

        private static bool HasAsyncRestResponse(MethodInfo m)
        {
            if (!m.ReturnType.IsConstructedGenericType)
            {
                var genericTypeDefinition = m.ReturnType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Task<>))
                {
                    var genericArgs = m.ReturnType.GetGenericArguments();
                    if (genericArgs.Any())
                    {
                        return genericArgs[0] == typeof(RestResponse);
                    }
                }
            }
            return false;
        }

        internal async Task<string> HandleRequestAsync(RestRequest req)
        {
            var cleanUrl = req.Uri?.TrimStart('/').TrimEnd('/');
            var incomingUriAsString = string.IsNullOrWhiteSpace(cleanUrl) ? string.Empty : "/" + cleanUrl;

            var parsedUri = UriParser.Parse(incomingUriAsString);
            var restMethod = _restMethodCollection.FirstOrDefault(r => r.Match(parsedUri));
            if (restMethod == null)
            {
                return "400: Bad request";
            }

            RestResponse result;
            try
            {
                var methodInvokeResult = ExecuteAnonymousMethod(restMethod, req, parsedUri);
                result = restMethod.Async ? await (Task<RestResponse>)methodInvokeResult : (RestResponse)methodInvokeResult;
            }
            catch (Exception e)
            {
                result = new RestResponse
                {
                    Sucesso = false,
                    ContentData = e.Message
                };
            }

            return new XElement("RestResponse",
                new XElement("Sucesso", result.Sucesso),
                new XElement("ContentData", result.ContentData)).ToString(SaveOptions.DisableFormatting);
        }

        private object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestRequest request, ParsedUri requestUri)
        {
            var instantiator = ObjectCache[info.MethodInfo.DeclaringType];

            object[] parameters;
            if (info.HasContentParameter && request.Content != null)
            {
                try
                {
                    var content = (XElement)request.Content;
                    var xmlSerializer = new XmlSerializer(info.ContentParameterType);
                    using (var reader = content.FirstNode.CreateReader())
                    {
                        object contentObj = xmlSerializer.Deserialize(reader);
                        parameters = info.GetParametersFromUri(requestUri).Concat(new[] { contentObj }).ToArray();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Ocorreu um erro ao desserializar o objeto de tipo {nameof(info.ContentParameterType)}", e);
                }
            }
            else
            {
                parameters = info.GetParametersFromUri(requestUri).ToArray();
            }

            return info.MethodInfo.Invoke(instantiator, parameters);
        }
    }
}
