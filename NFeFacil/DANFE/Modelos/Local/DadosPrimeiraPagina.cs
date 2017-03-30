using NFeFacil.DANFE.Modelos.Global;
using System.Collections.Generic;

namespace NFeFacil.DANFE.Modelos.Local
{
    public struct DadosPrimeiraPagina
    {
        public DadosCabecalho cabec;
        public DadosNFe nfe;
        public DadosCliente cliente;
        public DadosMotorista motorista;
        public DadosImposto imposto;
        public List<DadosProduto> Produto;
        public DadosAdicionais extras;
        public int paginaTotal;
        public DadosDuplicata[] duplicatas;
    }
}
