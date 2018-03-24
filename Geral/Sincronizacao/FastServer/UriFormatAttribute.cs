using System;

namespace NFeFacil.Sincronizacao.FastServer
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class UriFormatAttribute : Attribute
    {
        public string UriFormat { get; }

        public UriFormatAttribute(string uriFormat)
        {
            UriFormat = uriFormat;
        }
    }
}
