using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo30 : BaseRN
    {
        public int modBCST { get; set; }
        public double pICMSST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public string motDesICMS { get; set; }

        public Tipo30() { }
        public Tipo30(TelasRN.Tipo30 tela)
        {
            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;
            motDesICMS = tela.motDesICMS;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            bool usarpMVAST = TryParse(pMVAST, out double pMVASTd);
            bool usarpRedBCST = TryParse(pRedBCST, out double pRedBCSTd);
            var vBCST = (CalcularBC(prod) + ObterIPI(prod)) * (100 + pMVASTd) / 100;
            var valorSemReducao = vBCST * pICMSST / 100;
            vBCST *= 1 - (pRedBCSTd / 100);
            var vICMSST = vBCST * pICMSST / 100;

            var vICMSDeson = valorSemReducao - vICMSST;
            bool infDeson = vICMSDeson > 0 && !string.IsNullOrEmpty(motDesICMS);

            return new ICMS30()
            {
                CST = CST,
                modBCST = modBCST.ToString(),
                motDesICMS = infDeson ? motDesICMS : null,
                Orig = Origem,
                pICMSST = ToStr(pICMSST, "F4"),
                pMVAST = usarpMVAST ? ToStr(pMVASTd, "F4") : null,
                pRedBCST = usarpRedBCST ? ToStr(pRedBCSTd, "F4") : null,
                vBCST = ToStr(vBCST),
                vICMSDeson = infDeson ? ToStr(vICMSDeson) : null,
                vICMSST = ToStr(vICMSST)
            };
        }
    }
}
