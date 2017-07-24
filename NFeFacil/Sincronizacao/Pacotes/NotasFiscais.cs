using NFeFacil.ItensBD;
using System.Collections.Generic;

namespace NFeFacil.Sincronizacao.Pacotes
{
    public struct NotasFiscais : IPacote
    {
        public List<NFeDI> DIs { get; set; }
    }
}
