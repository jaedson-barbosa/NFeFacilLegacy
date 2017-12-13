using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    class Tipo51 : BaseRN
    {
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pRedBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public string vICMSOp { get; set; }
        public string pDif { get; set; }
        public string vICMSDif { get; set; }

        public Tipo51(TelasRN.Tipo51 tela)
        {
            modBC = tela.modBC;
            vBC = tela.vBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;
            vICMS = tela.vICMS;

            vICMSOp = tela.vICMSOp;
            pDif = tela.pDif;
            vICMSDif = tela.vICMSDif;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS51()
            {
                CST = CST,
                modBC = modBC.ToString(),
                Orig = Origem,
                pICMS = pICMS,
                pRedBC = pRedBC,
                vBC = vBC,
                vICMS = vICMS,
                pDif = pDif,
                vICMSDif = vICMSDif,
                vICMSOp = vICMSOp
            };
        }
    }
}
