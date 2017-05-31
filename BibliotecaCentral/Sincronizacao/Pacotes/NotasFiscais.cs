using BibliotecaCentral.ItensBD;
using System.Collections.Generic;

namespace BibliotecaCentral.Sincronizacao.Pacotes
{
    public struct NotasFiscais : IPacote
    {
        public List<NFeDI> DIs { get; set; }
    }
}
