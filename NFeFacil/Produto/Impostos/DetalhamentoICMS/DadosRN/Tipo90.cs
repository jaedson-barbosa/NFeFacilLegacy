using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo90 : BaseRN
    {
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pRedBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public string vICMSDeson { get; set; }
        public string motDesICMS { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public string vBCST { get; set; }
        public string pICMSST { get; set; }
        public string vICMSST { get; set; }

        public Tipo90() { }
        public Tipo90(TelasRN.Tipo90 tela)
        {
            modBC = tela.modBC;
            vBC = tela.vBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;
            vICMS = tela.vICMS;

            vICMSDeson = tela.vICMSDeson;
            motDesICMS = tela.motDesICMS;

            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            vBCST = tela.vBCST;
            pICMSST = tela.pICMSST;
            vICMSST = tela.vICMSST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS90()
            {
                CST = CST,
                modBC = modBC.ToString(),
                modBCST = modBCST.ToString(),
                motDesICMS = motDesICMS,
                Orig = Origem,
                pICMS = pICMS,
                pICMSST = pICMSST,
                pMVAST = pMVAST,
                pRedBC = pRedBC,
                pRedBCST = pRedBCST,
                vBC = vBC,
                vBCST = vBCST,
                vICMS = vICMS,
                vICMSDeson = vICMSDeson,
                vICMSST = vICMSST
            };
        }
    }
}
