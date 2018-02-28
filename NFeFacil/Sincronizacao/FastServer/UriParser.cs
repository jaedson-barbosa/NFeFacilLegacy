using System;

namespace NFeFacil.Sincronizacao.FastServer
{
    static class UriParser
    {
        public static ParsedUri Parse(string uriFormatUri)
        {
            var uriParts = uriFormatUri.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var pathParts = new PathPart[uriParts.Length];
            for (int i = 0; i < uriParts.Length; i++)
            {
                var pathPart = uriParts[i];
                var isArgument = pathPart.StartsWith("{") && pathPart.EndsWith("}");
                var type = isArgument ? PathPart.PathPartType.Argument : PathPart.PathPartType.Path;
                var value = isArgument ? pathPart.Substring(1, pathPart.Length - 2) : pathPart;
                pathParts[i] = new PathPart(type, value);
            }
            return new ParsedUri(pathParts);
        }
    }
}