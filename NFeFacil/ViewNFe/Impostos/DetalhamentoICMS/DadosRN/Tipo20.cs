using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

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

        void CalcularICMS(ref ICMS20 icms, DetalhesProdutos prod)
        {
            var vBC = CalcularBC(prod);
            var pICMS = Parse(icms.pICMS);
            var valorSemReducao = vBC * pICMS / 100;
            var pRedBC = Parse(icms.pRedBC);
            vBC *= 1 - (pRedBC / 100);
            icms.vBC = ToStr(vBC);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);

            var vICMSDeson = valorSemReducao - vICMS;
            if (vICMSDeson == 0)
            {
                icms.vICMSDeson = null;
                icms.motDesICMS = null;
            }
            else
            {
                icms.vICMSDeson = ToStr(vICMSDeson);
            }
        }
    }
}
