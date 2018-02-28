using System.Xml.Linq;

namespace NFeFacil.Sincronizacao.FastServer
{
    public sealed class RestRequest
    {
        public string Uri { get; set; }
        public XNode Content { get; set; }
    }
}