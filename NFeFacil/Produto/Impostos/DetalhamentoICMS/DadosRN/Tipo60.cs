using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo60 : BaseRN
    {
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }

        public Tipo60() { }
        public Tipo60(TelasRN.Tipo60 tela)
        {
            vBCSTRet = tela.vBCSTRet;
            vICMSSTRet = tela.vICMSSTRet;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS60()
            {
                CST = CST,
                Orig = Origem,
                vBCSTRet = vBCSTRet,
                vICMSSTRet = vICMSSTRet
            };
        }
    }
}
