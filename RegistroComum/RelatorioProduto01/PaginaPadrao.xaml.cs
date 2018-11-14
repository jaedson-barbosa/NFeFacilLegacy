using System;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace RegistroComum.RelatorioProduto01
{
    public sealed partial class PaginaPadrao : UserControl
    {
        DadosRelatorioProduto01 Dados;
        Action RequisitarMaisPaginas;
        int NumeroPagina;

        internal PaginaPadrao(DadosRelatorioProduto01 dados, Action requisitarMaisPaginas, int numPagina)
        {
            InitializeComponent();
            Dados = dados;
            RequisitarMaisPaginas = requisitarMaisPaginas;
            NumeroPagina = numPagina;
        }

        private void stkContent_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            int prodsRestantes = Dados.Produtos.Sum(x => x.Value.Count(y => !y.Adicionado));
            var par = Dados.Produtos.First(x => x.Value.Any(y => !y.Adicionado));
            if (!par.Value.Any(x => x.Adicionado))
                stkContent.Children.Add(new InfoTabela(par.Key.Categoria, par.Key.Fornecedor));
            var tabelaAtual = new TabelaSimples(!par.Value.Any(x => x.Adicionado));
            stkContent.Children.Add(tabelaAtual);
            while (prodsRestantes > 0 && espacoRestante.ActualHeight >= 28)
            {
                if (par.Value.Any(x => !x.Adicionado))
                {
                    tabelaAtual.Produtos.Add(par.Value.First(x => !x.Adicionado));
                    prodsRestantes--;
                }
                else
                {
                    par = Dados.Produtos.First(x => x.Value.Any(y => !y.Adicionado));
                    if (!par.Value.Any(x => x.Adicionado))
                        stkContent.Children.Add(new InfoTabela(par.Key.Categoria, par.Key.Fornecedor));
                    tabelaAtual = new TabelaSimples(!par.Value.Any(x => x.Adicionado));
                    stkContent.Children.Add(tabelaAtual);
                }
            }
            if (prodsRestantes > 0)
                RequisitarMaisPaginas();
        }
    }
}
