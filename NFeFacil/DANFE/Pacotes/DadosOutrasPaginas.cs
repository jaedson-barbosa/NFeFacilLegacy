using System.Collections.Generic;

namespace NFeFacil.DANFE.Pacotes
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
