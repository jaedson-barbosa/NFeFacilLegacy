using BibliotecaCentral.ItensBD;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BibliotecaCentral.Sincronizacao.Pacotes
{
    public struct NotasFiscais : IPacote
    {
        public List<NFeDI> DIs { get; set; }
        public List<XElement> XMLs { get; set; }
    }
}
