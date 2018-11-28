using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace RegistroComum.RelatorioProduto01
{
    public sealed partial class PaginaPadrao : UserControl
    {
        Dictionary<ParCategoriaFornecedor, List<ExibicaoProduto>> Dados;
        Action RequisitarMaisPaginas;
        int NumeroPagina;

        internal PaginaPadrao(Dictionary<ParCategoriaFornecedor, List<ExibicaoProduto>> dados, Action requisitarMaisPaginas, int numPagina)
        {
            InitializeComponent();
            Dados = dados;
            RequisitarMaisPaginas = requisitarMaisPaginas;
            NumeroPagina = numPagina;
        }

        private void stkContent_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            int prodsRestantes = Dados.Sum(x => x.Value.Count(y => !y.Adicionado)),
                quantSuportada = 51;
            var par = Dados.First(x => x.Value.Any(y => !y.Adicionado));
            if (!par.Value.Any(x => x.Adicionado))
            {
                stkContent.Children.Add(new InfoTabela(par.Key.Categoria, par.Key.Fornecedor));
                quantSuportada -= 1;
            }
            var tabelaAtual = new TabelaSimples();
            stkContent.Children.Add(tabelaAtual);
            while (prodsRestantes > 0 && quantSuportada-- > 0)
            { 
                if (par.Value.Any(x => !x.Adicionado))
                {
                    var prod = par.Value.First(x => !x.Adicionado);
                    prod.Adicionado = true;
                    tabelaAtual.Produtos.Add(prod);
                    prodsRestantes--;
                }
                else
                {
                    par = Dados.First(x => x.Value.Any(y => !y.Adicionado));
                    if (!par.Value.Any(x => x.Adicionado))
                        stkContent.Children.Add(new InfoTabela(par.Key.Categoria, par.Key.Fornecedor));
                    tabelaAtual = new TabelaSimples();
                    stkContent.Children.Add(tabelaAtual);
                    quantSuportada -=2;
                }
            }
            if (prodsRestantes > 0)
                RequisitarMaisPaginas();
        }
    }
}
