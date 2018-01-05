using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo20 : BaseRN
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }

        public double pRedBC { get; set; }
        public string motDesICMS { get; set; }

        public Tipo20() { }
        public Tipo20(TelasRN.Tipo20 tela)
        {
            modBC = tela.modBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;
            motDesICMS = tela.motDesICMS;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            var vBC = CalcularBC(prod);
            var valorSemReducao = vBC * pICMS / 100;
            vBC *= 1 - (pRedBC / 100);
            var vICMS = vBC * pICMS / 100;

            var vICMSDeson = valorSemReducao - vICMS;
            bool infDeson = vICMSDeson > 0 && !string.IsNullOrEmpty(motDesICMS);

            return new ICMS20()
            {
                CST = CST,
                modBC = modBC.ToString(),
                motDesICMS = infDeson ? motDesICMS : null,
                Orig = Origem,
                pICMS = ToStr(pICMS, "F4"),
                vBC = ToStr(vBC),
                vICMS = ToStr(vICMS),
                vICMSDeson = infDeson ? ToStr(vICMSDeson) : null,
                pRedBC = ToStr(pRedBC, "F4")
            };
        }
    }
}
