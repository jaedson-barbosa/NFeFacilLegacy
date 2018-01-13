using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS.DadosRN
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
            return new ICMSST()
            {
                CST = "41",
                Orig = Origem,
                vBCSTDest = vBCSTDest,
                vBCSTRet = vBCSTRet,
                vICMSSTDest = vICMSSTDest,
                vICMSSTRet = vICMSSTRet
            };
        }
    }
}
