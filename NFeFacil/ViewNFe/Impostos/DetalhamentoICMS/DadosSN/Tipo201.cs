using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosSN
{
    class Tipo201 : BaseSN
    {
        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }

        public int modBCST { get; private set; }
        public string pMVAST { get; private set; }
        public string pRedBCST { get; private set; }
        public string vBCST { get; private set; }
        public string pICMSST { get; private set; }
        public string vICMSST { get; private set; }

        public Tipo201(TelasSN.Tipo201 tela)
        {
            pCredSN = tela.pCredSN;
            vCredICMSSN = tela.vCredICMSSN;

            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            vBCST = tela.vBCST;
            pICMSST = tela.pICMSST;
            vICMSST = tela.vICMSST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMSSN201()
            {
                CSOSN = CSOSN,
                modBCST = modBCST.ToString(),
                Orig = Origem,
                pCredSN = pCredSN,
                pICMSST = pICMSST,
                pMVAST = pMVAST,
                pRedBCST = pRedBCST,
                vBCST = vBCST,
                vCredICMSSN = vCredICMSSN,
                vICMSST = vICMSST
            };
        }

        void CalcularICMS(ref ICMSSN201 icms, DetalhesProdutos prod)
        {
            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = CalcularBC(prod) * (100 + pMVAST) / 100;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = vBCST * pICMSST / 100;
            icms.vICMSST = ToStr(vICMSST);
        }
    }
}
