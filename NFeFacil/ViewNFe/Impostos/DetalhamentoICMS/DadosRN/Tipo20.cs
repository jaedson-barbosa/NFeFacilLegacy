using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    class Tipo20 : BaseRN
    {
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pRedBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public string vICMSDeson { get; set; }
        public string motDesICMS { get; set; }

        public Tipo20(TelasRN.Tipo20 tela)
        {
            modBC = tela.modBC;
            vBC = tela.vBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;
            vICMS = tela.vICMS;

            vICMSDeson = tela.vICMSDeson;
            motDesICMS = tela.motDesICMS;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS20()
            {
                CST = CST,
                modBC = modBC.ToString(),
                motDesICMS = motDesICMS,
                Orig = Origem,
                pICMS = pICMS,
                vBC = vBC,
                vICMS = vICMS,
                vICMSDeson = vICMSDeson,
                pRedBC = pRedBC
            };
        }
    }
}
