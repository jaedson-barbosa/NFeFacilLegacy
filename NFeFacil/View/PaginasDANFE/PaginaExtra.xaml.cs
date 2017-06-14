using NFeFacil.DANFE.Pacotes;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.PaginasDANFE
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class PaginaExtra : UserControl, IPagina
    {
        double LarguraPagina => PartesDANFE.DimensoesPadrao.CentimeterToPixel(21);
        double AlturaPagina => PartesDANFE.DimensoesPadrao.CentimeterToPixel(29.7);
        Thickness MargemPadrao => new Thickness(PartesDANFE.DimensoesPadrao.CentimeterToPixel(1));

        DadosNFe ContextoNFe { get; }

        public PaginaExtra(DadosNFe cabecalho, IEnumerable<DadosProduto> produtos, RichTextBlock infoAdicional, UIElementCollection paiPaginas, MotivoCriacaoPaginaExtra motivo, PaginaPrincipal principal)
        {
            this.InitializeComponent();
            ContextoNFe = cabecalho;
            if (motivo == MotivoCriacaoPaginaExtra.Ambos)
            {
                double total = 0, maximo = espacoParaProdutos.ActualHeight;
                var produtosNestaPagina = produtos.TakeWhile(x =>
                {
                    var item = new PartesDANFE.ItemProduto() { DataContext = x };
                    item.Measure(new Windows.Foundation.Size(PartesDANFE.DimensoesPadrao.LarguraTotalStatic, espacoParaProdutos.ActualHeight));
                    total += item.DesiredSize.Height;
                    return total <= maximo;
                });
                campoProdutos.DataContext = produtosNestaPagina.ToArray();

                bool excessoProdutos = produtos.Count() - produtosNestaPagina.Count() > 0;

                if (excessoProdutos)
                {
                    var produtosRestantes = produtos.Except(produtosNestaPagina);
                    paiPaginas.Add(new PaginaExtra(cabecalho, produtosRestantes, infoAdicional, paiPaginas, MotivoCriacaoPaginaExtra.Ambos, principal));
                }
                else
                {
                    infoAdicional.OverflowContentTarget = campoInfo;
                    if (infoAdicional.HasOverflowContent)
                    {
                        infoAdicional.OverflowContentTarget = null;
                        grd.Children.Remove(geralCampoInfo);
                        paiPaginas.Add(new PaginaExtra(cabecalho, null, infoAdicional, paiPaginas, MotivoCriacaoPaginaExtra.Observacao, principal));
                    }
                }
            }
            else if (motivo == MotivoCriacaoPaginaExtra.Observacao)
            {
                grd.Children.Remove(campoProdutos);
                espacoParaProdutos.Height = new GridLength(0);
                infoAdicional.OverflowContentTarget = campoInfo;
            }
            else
            {
                grd.Children.Remove(geralCampoInfo);

                double total = 0, maximo = espacoParaProdutos.ActualHeight;
                var produtosNestaPagina = produtos.TakeWhile(x =>
                {
                    var item = new PartesDANFE.ItemProduto() { DataContext = x };
                    item.Measure(new Windows.Foundation.Size(PartesDANFE.DimensoesPadrao.LarguraTotalStatic, espacoParaProdutos.ActualHeight));
                    total += item.DesiredSize.Height;
                    return total <= maximo;
                });
                campoProdutos.DataContext = produtosNestaPagina.ToArray();

                bool excessoProdutos = produtos.Count() - produtosNestaPagina.Count() > 0;

                if (excessoProdutos)
                {
                    var produtosRestantes = produtos.Except(produtosNestaPagina);
                    paiPaginas.Add(new PaginaExtra(cabecalho, produtosRestantes, infoAdicional, paiPaginas, MotivoCriacaoPaginaExtra.Produtos, principal));
                }
            }
            principal.OnPaginaCarregada();
        }

        public void DefinirPagina(int atual, int total)
        {
            
        }
    }
}
