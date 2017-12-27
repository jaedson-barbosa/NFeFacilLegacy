using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo51 : BaseRN
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }

        public double pRedBC { get; set; }
        public double pDif { get; set; }
        public bool Calcular { get; set; }

        public Tipo51() { }
        public Tipo51(TelasRN.Tipo51 tela)
        {
            modBC = tela.modBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;
            pDif = tela.pDif;
            Calcular = tela.Calcular;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            if (Calcular)
            {
                var vBC = CalcularBC(prod);
                vBC *= 1 - (pRedBC / 100);
                var vICMSOp = vBC * pICMS / 100;
                var vICMSDif = vBC * (100 - pDif) / 100;
                var vICMS = vICMSOp - vICMSDif;

                return new ICMS51()
                {
                    CST = CST,
                    modBC = modBC.ToString(),
                    Orig = Origem,
                    pICMS = ToStr(pICMS, "F4"),
                    pRedBC = ToStr(pRedBC, "F4"),
                    vBC = ToStr(vBC),
                    vICMS = ToStr(vICMS),
                    pDif = ToStr(pDif, "F4"),
                    vICMSDif = ToStr(vICMSDif),
                    vICMSOp = ToStr(vICMSOp)
                };
            }
            else
            {
                return new ICMS51()
                {
                    CST = CST,
                    Orig = Origem
                };
            }
        }
    }
}
