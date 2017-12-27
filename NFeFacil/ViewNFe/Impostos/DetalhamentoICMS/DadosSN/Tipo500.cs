using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosSN
{
    public class Tipo500 : BaseSN
    {
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }

        public Tipo500() { }
        public Tipo500(TelasSN.Tipo500 tela)
        {
            vBCSTRet = tela.vBCSTRet;
            vICMSSTRet = tela.vICMSSTRet;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMSSN500()
            {
                CSOSN = CSOSN,
                Orig = Origem,
                vBCSTRet = vBCSTRet,
                vICMSSTRet = vICMSSTRet
            };
        }
    }
}
