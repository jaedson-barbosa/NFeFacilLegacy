using System.Xml.Linq;

namespace BaseGeral.Sincronizacao.FastServer
{
    public sealed class RestRequest
    {
        public string Uri { get; set; }
        public XNode Content { get; set; }
    }
}