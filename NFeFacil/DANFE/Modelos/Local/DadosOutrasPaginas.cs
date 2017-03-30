using NFeFacil.DANFE.Modelos.Global;
using System.Collections.Generic;

namespace NFeFacil.DANFE.Modelos.Local
{
    public struct DadosOutrasPaginas
    {
        public DadosNFe nfe;
        public DadosCliente cliente;
        public List<DadosProduto> Produto;
        public DadosAdicionais extras;
        public int paginaAtual;
        public int paginaTotal;
    }
}
