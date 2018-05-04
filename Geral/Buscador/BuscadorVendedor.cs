using BaseGeral.ItensBD;
using BaseGeral.View;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace BaseGeral.Buscador
{
    public sealed class BuscadorVendedor : BaseBuscador<ConjuntoBasicoExibicao<Vendedor>>
    {
        public BuscadorVendedor()
        {
            using (var repo = new Repositorio.Leitura())
            {
                TodosItens = repo.ObterVendedores().Select(atual => new ConjuntoBasicoExibicao<Vendedor>
                {
                    Objeto = atual.Item1,
                    Principal = atual.Item1.Nome,
                    Secundario = ExtensoesPrincipal.AplicarMáscaraDocumento(atual.Item1.CPFStr),
                    Imagem = atual.Item2?.GetSource()
                }).OrderBy(x => x.Principal).ToArray();
                Itens = TodosItens.GerarObs();
            }
        }

        protected override (string, string) ItemComparado(ConjuntoBasicoExibicao<Vendedor> item, int modoBusca)
        {
            switch (modoBusca)
            {
                case 0: return (item.Principal, null);
                case 1: return (item.Objeto.CPFStr, null);
                default: return (item.Principal, item.Objeto.CPFStr);
            }
        }

        protected override void InvalidarItem(ConjuntoBasicoExibicao<Vendedor> item, int modoBusca)
        {
            if (DefinicoesPermanentes.ModoBuscaProduto == 0) item.Principal = InvalidProduct;
            else item.Objeto.CPFStr = InvalidProduct;
        }

        public void AtualizarImagem(ImageSource imagem, ConjuntoBasicoExibicao<Vendedor> vendedor)
        {
            var index = Itens.IndexOf(vendedor);
            vendedor.Imagem = imagem;
            Itens[index] = vendedor;
        }
    }
}
