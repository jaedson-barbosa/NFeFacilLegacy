using BibliotecaCentral.ItensBD;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BibliotecaCentral.Sincronizacao.Pacotes
{
    public sealed class NotasFiscais : PacoteBase
    {
        public Dictionary<NFeDI, XElement> Duplas { get; set; }
    }
}
