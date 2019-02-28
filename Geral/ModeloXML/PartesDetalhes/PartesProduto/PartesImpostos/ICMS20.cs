using static BaseGeral.ExtensoesPrincipal;
namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS20 : ComumICMS, IRegimeNormal
    {
        public ICMS20()
        {
        }

        public ICMS20(int origem, string cst, int modBC, double pICMS,
            double pRedBC, string motDesICMS, DetalhesProdutos prod) : base(origem, cst, false)
        {
            var vBC = CalcularBC(prod);
            var valorSemReducao = vBC * pICMS / 100;
            vBC *= 1 - (pRedBC / 100);
            var vICMS = vBC * pICMS / 100;

            var vICMSDeson = valorSemReducao - vICMS;
            bool infDeson = vICMSDeson > 0 && !string.IsNullOrEmpty(motDesICMS);

            this.modBC = modBC.ToString();
            this.vBC = ToStr(vBC);
            this.pICMS = ToStr(pICMS, "F4");
            this.vICMS = ToStr(vICMS);

            this.pRedBC = ToStr(pRedBC, "F4");
            this.vICMSDeson = infDeson ? ToStr(vICMSDeson) : null;
            this.motDesICMS = infDeson ? motDesICMS : null;
        }
    }
}
