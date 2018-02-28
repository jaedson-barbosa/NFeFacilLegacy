using NFeFacil.ItensBD;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.Produto.Impostos;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.Produto
{
    class DadosAdicaoProduto
    {
        public DadosAdicaoProduto(ProdutoDI auxiliar)
        {
            Completo = new DetalhesProdutos
            {
                Produto = auxiliar.ToProdutoOuServico()
            };
            Auxiliar = auxiliar;
            ImpostosPadrao = auxiliar.GetImpostosPadrao();
        }

        public DadosAdicaoProduto(ProdutoDI auxiliar, DetalhesProdutos completo)
        {
            Completo = completo;
            Auxiliar = auxiliar;
            ImpostosPadrao = auxiliar.GetImpostosPadrao();
        }

        public DetalhesProdutos Completo { get; }
        public ProdutoDI Auxiliar { get; }
        public (PrincipaisImpostos Tipo, string NomeTemplate, int CST)[] ImpostosPadrao { get; }
        public bool IsNFCe { get; set; }

        public List<ImpostoEscolhivel> GetImpostosPadraoNFe(bool produto)
        {
            List<ImpostoEscolhivel> impostos;
            if (produto)
            {
                impostos = new List<ImpostoEscolhivel>
                {
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.ICMS)),
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.IPI)),
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.PIS)),
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.COFINS)),
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.II)),
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.ICMSUFDest))
                };
                var icmsArmazenado = Auxiliar.GetICMSArmazenados();
                if (icmsArmazenado != null && icmsArmazenado.Count() > 0)
                {
                    var icms = icmsArmazenado.Select(x => new ImpostoEscolhivel(x));
                    impostos.AddRange(icms);
                }
            }
            else
            {
                impostos = new List<ImpostoEscolhivel>
                {
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.IPI)),
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.PIS)),
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.COFINS)),
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.ISSQN)),
                    new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.ICMSUFDest))
                };
            }
            var impsArmazenado = Auxiliar.GetImpSimplesArmazenados();
            if (impsArmazenado != null && impsArmazenado.Count() > 0)
            {
                var imps = impsArmazenado.Select(x => new ImpostoEscolhivel(x));
                impostos.AddRange(imps);
            }

            int i = 0;
            impostos.ForEach(x => x.Id = i++);
            return impostos;
        }

        public List<ImpostoEscolhivel> GetImpostosPadraoNFCe()
        {
            List<ImpostoEscolhivel> impostos = new List<ImpostoEscolhivel>
            {
                new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.ICMS)),
                new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.PIS)),
                new ImpostoEscolhivel(new ImpostoPadrao(PrincipaisImpostos.COFINS)),
            };
            var icmsArmazenado = Auxiliar.GetICMSArmazenados();
            if (icmsArmazenado != null && icmsArmazenado.Count() > 0)
            {
                var icms = icmsArmazenado.Select(x => new ImpostoEscolhivel(x));
                impostos.AddRange(icms);
            }

            var impsArmazenado = Auxiliar.GetImpSimplesArmazenados();
            if (impsArmazenado != null && impsArmazenado.Count() > 0)
            {
                impostos.AddRange(from x in impsArmazenado
                                  where x.Tipo != PrincipaisImpostos.IPI
                                  select new ImpostoEscolhivel(x));
            }

            int i = 0;
            impostos.ForEach(x => x.Id = i++);
            return impostos;
        }
    }
}
