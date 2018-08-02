using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Venda.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo60 : BaseRN
    {
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }
        public string pST { get; set; }

        public Tipo60() { }
        public Tipo60(TelasRN.Tipo60 tela)
        {
            vBCSTRet = tela.vBCSTRet;
            vICMSSTRet = tela.vICMSSTRet;
            pST = tela.pST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS60()
            {
                CST = CST,
                Orig = Origem,
                vBCSTRet = vBCSTRet,
                vICMSSTRet = vICMSSTRet,
                pST = pST
            };
        }
    }
}
