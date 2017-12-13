using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    class Tipo0 : BaseRN
    {
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public Tipo0(TelasRN.Tipo0 tela)
        {
            modBC = tela.modBC;
            vBC = tela.vBC;
            pICMS = tela.pICMS;
            vICMS = tela.vICMS;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS00()
            {
                CST = CST,
                modBC = modBC.ToString(),
                Orig = Origem,
                pICMS = pICMS,
                vBC = vBC,
                vICMS = vICMS
            };
        }

        void CalcularICMS(ref ICMS00 icms, DetalhesProdutos prod)
        {
            var vBC = CalcularBC(prod);
            var pICMS = Parse(icms.pICMS);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);
        }
    }
}
