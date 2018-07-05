using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BaseGeral.Sincronizacao.FastServer
{
    class RestControllerMethodInfo
    {
        readonly IEnumerable<Type> _validParameterTypes;
        readonly ParsedUri _matchUri;
        readonly IEnumerable<ParameterValueGetter> _parameterGetters;

        internal MethodInfo MethodInfo { get; }
        internal bool HasContentParameter { get; }
        internal Type ContentParameterType { get; }
        internal bool Async { get; }

        internal RestControllerMethodInfo(MethodInfo methodInfo, bool async)
        {
            _matchUri = GetUriFromMethod(methodInfo);
            Async = async;
            MethodInfo = methodInfo;
            _validParameterTypes = GetValidParameterTypes();
            _parameterGetters = GetParameterGetters(methodInfo);
            HasContentParameter = TryGetContentParameterType(methodInfo, out Type contentParameterType);
            ContentParameterType = contentParameterType;
        }

        static ParsedUri GetUriFromMethod(MethodInfo methodInfo)
        {
            var uriFormatter = methodInfo.GetCustomAttribute<UriFormatAttribute>();
            return UriParser.Parse(uriFormatter.UriFormat);
        }

        static Type[] GetValidParameterTypes()
        {
            return new[] { typeof(string), typeof(decimal), typeof(double),
                typeof(float), typeof(short), typeof(int), typeof(long),
                typeof(byte), typeof(bool), typeof(DateTime), typeof(char),
                typeof(sbyte), typeof(ushort), typeof(uint), typeof(ulong) };
        }

        static bool TryGetContentParameterType(MethodInfo methodInfo, out Type content)
        {
            var fromContentParameter = methodInfo.GetParameters().FirstOrDefault(p => p.GetCustomAttribute<FromContentAttribute>() != null);
            if (fromContentParameter != null)
            {
                content = fromContentParameter.ParameterType;
                return true;
            }

            content = null;
            return false;
        }

        private ParameterValueGetter[] GetParameterGetters(MethodInfo methodInfo)
        {
            var fromUriParams = (from p in methodInfo.GetParameters()
                                 where p.GetCustomAttribute<FromContentAttribute>() == null
                                 select p).ToList();

            if (!ParametersHaveValidType(fromUriParams.Select(p => p.ParameterType)))
            {
                throw new InvalidOperationException("Can't use method parameters with a custom type.");
            }

           return fromUriParams.Select(x => GetParameterGetter(x, _matchUri)).ToArray();
        }

        private static ParameterValueGetter GetParameterGetter(ParameterInfo parameterInfo, ParsedUri matchUri)
        {
            var methodName = parameterInfo.Name;
            var firstPathPartMatch = matchUri.PathParts
                .Select((x, i) => new { Part = x, Index = i })
                .Where(x => x.Part.PartType == PathPart.PathPartType.Argument)
                .FirstOrDefault(x => methodName.Equals(x.Part.Value, StringComparison.OrdinalIgnoreCase));

            if (firstPathPartMatch != null)
                return new ParameterValueGetter(parameterInfo.ParameterType, firstPathPartMatch.Index);

            throw new Exception($"Method {methodName} not found in rest controller method uri {matchUri}.");
        }

        private bool ParametersHaveValidType(IEnumerable<Type> parameters)
        {
            return !parameters.Except(_validParameterTypes).Any();
        }

        internal bool Match(ParsedUri uri)
        {
            if (_matchUri.PathParts.Count != uri.PathParts.Count)
            {
                return false;
            }

            for (var i = 0; i < _matchUri.PathParts.Count; i++)
            {
                var fromPart = _matchUri.PathParts[i];
                var toPart = uri.PathParts[i];
                if (fromPart.PartType == PathPart.PathPartType.Argument)
                {
                    continue;
                }
                else if (!fromPart.Value.Equals(toPart.Value, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        internal IEnumerable<object> GetParametersFromUri(ParsedUri uri)
        {
            return _parameterGetters.Select(x => x.GetParameterValue(uri)).ToArray();
        }

        public override string ToString()
        {
            return _matchUri.ToString();
        }
    }
}
