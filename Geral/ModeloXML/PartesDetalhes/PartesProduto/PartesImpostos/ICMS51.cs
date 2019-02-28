using static BaseGeral.ExtensoesPrincipal;
namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS51 : ComumICMS, IRegimeNormal
    {
        public ICMS51()
        {
        }

        public ICMS51(int origem, string cst) : base(origem, cst, false) { }

        public ICMS51(int origem, string cst, int modBC, double pICMS,
            double pRedBC, double pDif, DetalhesProdutos prod) : base(origem, cst, false)
        {
            var vBC = CalcularBC(prod);
            vBC *= 1 - (pRedBC / 100);
            var vICMSOp = vBC * pICMS / 100;
            var vICMSDif = vBC * (100 - pDif) / 100;
            var vICMS = vICMSOp - vICMSDif;

            this.modBC = modBC.ToString();
            this.vBC = ToStr(vBC);
            this.pICMS = ToStr(pICMS, "F4");
            this.vICMS = ToStr(vICMS);

            this.pRedBC = ToStr(pRedBC, "F4");
            this.vICMSOp = ToStr(vICMSOp);
            this.pDif = ToStr(pDif, "F4");
            this.vICMSDif = ToStr(vICMSDif);
        }
    }
}
