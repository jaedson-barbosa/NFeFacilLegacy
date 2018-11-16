using BaseGeral;
using BaseGeral.ItensBD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed partial class AssociarCategoriaFornecedor : ContentDialog
    {
        readonly CategoriaDI Categoria;
        ObservableCollection<FornecedorDI> FornecedoresDisponiveis { get; }
        FornecedorDI Escolhido;
        bool AlterarTodosProdutos;

        public AssociarCategoriaFornecedor(CategoriaDI categoria)
        {
            Categoria = categoria;
            using (var leitor = new BaseGeral.Repositorio.Leitura())
                FornecedoresDisponiveis = leitor.ObterFornecedores().GerarObs();
            Escolhido = FornecedoresDisponiveis[0];
            InitializeComponent();
        }

        void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ProdutoDI[] produtos;
            using (var leitor = new BaseGeral.Repositorio.Leitura())
                produtos = leitor.ObterProdutos().ToArray();
            using (var escritor = new BaseGeral.Repositorio.Escrita())
                for (int i = 0; i < produtos.Length; i++)
                {
                    bool fornVazio = produtos[i].IdFornecedor == default(Guid);
                    if (produtos[i].IdCategoria == Categoria.Id &&
                        (fornVazio || AlterarTodosProdutos))
                    {
                        produtos[i].IdFornecedor = Escolhido.Id;
                        escritor.SalvarItemSimples(produtos[i], DefinicoesTemporarias.DateTimeNow);
                    }
                }
        }
    }
}
