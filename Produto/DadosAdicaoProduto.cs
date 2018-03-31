using BaseGeral.ItensBD;
using BaseGeral.ModeloXML.PartesDetalhes;
using Venda.Impostos;
using System.Collections.Generic;
using System.Linq;

namespace Venda
{
    public sealed class DadosAdicaoProduto
    {
        public DadosAdicaoProduto(ProdutoDI auxiliar)
        {
            Completo = new DetalhesProdutos
            {
                Produto = auxiliar.ToProdutoOuServico()
            };
            Auxiliar = auxiliar;
            ImpostosPadrao = Auxiliar.GetImpostosPadrao();
        }

        public DadosAdicaoProduto(ProdutoDI auxiliar, DetalhesProdutos completo)
        {
            Completo = completo;
            Auxiliar = auxiliar;
            ImpostosPadrao = Auxiliar.GetImpostosPadrao();
        }

        public DetalhesProdutos Completo { get; }
        public ProdutoDIExtended Auxiliar { get; }
        public (PrincipaisImpostos Tipo, string NomeTemplate, int CST)[] ImpostosPadrao { get; }
        public bool IsNFCe { get; set; }

        public List<ImpostoArmazenado> GetImpostosPadraoNFe(bool produto)
        {
            List<ImpostoArmazenado> impostos;
            if (produto)
            {
                impostos = new List<ImpostoArmazenado>
                {
                    new ImpostoPadrao(PrincipaisImpostos.ICMS),
                    new ImpostoPadrao(PrincipaisImpostos.IPI),
                    new ImpostoPadrao(PrincipaisImpostos.PIS),
                    new ImpostoPadrao(PrincipaisImpostos.COFINS),
                    new ImpostoPadrao(PrincipaisImpostos.II),
                    new ImpostoPadrao(PrincipaisImpostos.ICMSUFDest)
                };
                var icmsArmazenado = Auxiliar.GetICMSArmazenados();
                if (icmsArmazenado != null && icmsArmazenado.Count() > 0)
                {
                    impostos.AddRange(icmsArmazenado);
                }
            }
            else
            {
                impostos = new List<ImpostoArmazenado>
                {
                    new ImpostoPadrao(PrincipaisImpostos.IPI),
                    new ImpostoPadrao(PrincipaisImpostos.PIS),
                    new ImpostoPadrao(PrincipaisImpostos.COFINS),
                    new ImpostoPadrao(PrincipaisImpostos.ISSQN),
                    new ImpostoPadrao(PrincipaisImpostos.ICMSUFDest)
                };
            }
            var impsArmazenado = Auxiliar.GetImpSimplesArmazenados();
            if (impsArmazenado != null && impsArmazenado.Count() > 0)
            {
                impostos.AddRange(impsArmazenado);
            }
            return impostos;
        }

        public List<ImpostoArmazenado> GetImpostosPadraoNFCe()
        {
            var impostos = new List<ImpostoArmazenado>
            {
                new ImpostoPadrao(PrincipaisImpostos.ICMS),
                new ImpostoPadrao(PrincipaisImpostos.PIS),
                new ImpostoPadrao(PrincipaisImpostos.COFINS)
            };
            var icmsArmazenado = Auxiliar.GetICMSArmazenados();
            if (icmsArmazenado != null && icmsArmazenado.Count() > 0)
            {
                impostos.AddRange(icmsArmazenado);
            }

            var impsArmazenado = Auxiliar.GetImpSimplesArmazenados();
            if (impsArmazenado != null && impsArmazenado.Count() > 0)
            {
                impostos.AddRange(from x in impsArmazenado
                                  where x.Tipo != PrincipaisImpostos.IPI
                                  select x);
            }
            return impostos;
        }
    }
}
