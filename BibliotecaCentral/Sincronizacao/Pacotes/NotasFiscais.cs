using System.Xml.Linq;

namespace BibliotecaCentral.Sincronizacao.Pacotes
{
    public sealed class NotasFiscais : PacoteBase
    {
        public XElement[] XMLs { get; set; }
    }
}
