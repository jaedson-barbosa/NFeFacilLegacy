using static BaseGeral.ExtensoesPrincipal;
namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS00 : ComumICMS, IRegimeNormal
    {
        public ICMS00(int origem, string cst, int modBC, double pICMS, DetalhesProdutos prod) : base(origem, cst, false)
        {
            var vBC = CalcularBC(prod);
            var vICMS = vBC * pICMS / 100;

            this.modBC = modBC.ToString();
            this.vBC = ToStr(vBC);
            this.pICMS = ToStr(pICMS, "F4");
            this.vICMS = ToStr(vICMS);
        }
    }
}
