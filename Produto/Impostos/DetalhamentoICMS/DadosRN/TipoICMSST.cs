using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Venda.Impostos.DetalhamentoICMS.DadosRN
{
    public class TipoICMSST : BaseRN
    {
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }
        public string vBCSTDest { get; set; }
        public string vICMSSTDest { get; set; }

        public TipoICMSST() { }
        public TipoICMSST(TelasRN.TipoICMSST tela)
        {
            vBCSTRet = tela.vBCSTRet;
            vICMSSTRet = tela.vICMSSTRet;
            vBCSTDest = tela.vBCSTDest;
            vICMSSTDest = tela.vICMSSTDest;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMSST(Origem, "41", vBCSTRet, vICMSSTRet, vBCSTDest, vICMSSTDest);
        }
    }
}
