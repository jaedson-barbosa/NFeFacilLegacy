using System;

namespace BaseGeral.Sincronizacao.FastServer
{
    struct ParameterValueGetter
    {
        readonly Type _parameterType;
        readonly int _pathIndex;

        public ParameterValueGetter(Type parameterType, int pathIndex)
        {
            _parameterType = parameterType;
            _pathIndex = pathIndex;
        }

        internal object GetParameterValue(ParsedUri parsedUri)
        {
            var value = parsedUri.PathParts[_pathIndex].Value;
            if (_parameterType == typeof(string))
            {
                var unescapedParameterValue = Uri.UnescapeDataString(value);
                return Convert.ChangeType(unescapedParameterValue, _parameterType);
            }
            return Convert.ChangeType(value, _parameterType);
        }
    }
}