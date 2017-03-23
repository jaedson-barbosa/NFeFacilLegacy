using System.Collections.Generic;
using System.Xml.Linq;

namespace NFeFacil.Sincronizacao.Pacotes
{
    public sealed class NotasFiscais : PacoteBase
    {
        public IEnumerable<XElement> XMLs { get; set; }
    }
}
