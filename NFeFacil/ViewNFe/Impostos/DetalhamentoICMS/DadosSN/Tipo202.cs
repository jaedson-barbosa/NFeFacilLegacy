using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosSN
{
    class Tipo202 : BaseSN
    {
        public int modBCST { get; private set; }
        public string pMVAST { get; private set; }
        public string pRedBCST { get; private set; }
        public double pICMSST { get; private set; }

        public Tipo202(TelasSN.Tipo202 tela)
        {
            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            var pMVASTd = string.IsNullOrEmpty(pMVAST) ? 0 : Parse(pMVAST);
            var pRedBCSTd = string.IsNullOrEmpty(pRedBCST) ? 0 : Parse(pRedBCST);
            var vBCST = CalcularBC(prod) * (100 + pMVASTd) / 100;
            vBCST *= 1 - (pRedBCSTd / 100);
            var vICMSST = vBCST * pICMSST / 100;

            return new ICMSSN202()
            {
                CSOSN = CSOSN,
                modBCST = modBCST.ToString(),
                Orig = Origem,
                pICMSST = ToStr(pICMSST, "F4"),
                pMVAST = string.IsNullOrEmpty(pMVAST) ? null : ToStr(pMVASTd, "F4"),
                pRedBCST = string.IsNullOrEmpty(pRedBCST) ? null : ToStr(pRedBCSTd, "F4"),
                vBCST = ToStr(vBCST),
                vICMSST = ToStr(vICMSST)
            };
        }
    }
}
