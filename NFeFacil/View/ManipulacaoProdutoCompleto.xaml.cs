using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ViewModel;
using NFeFacil.ViewModel.NotaFiscal;
using NFeFacil.ViewModel.PartesProdutoCompleto.ImpostosProduto;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoProdutoCompleto : Page
    {
        public ManipulacaoProdutoCompleto()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var Produto = e.Parameter as DetalhesProdutos;
            DataContext = new ProdutoCompletoDataContext(Produto);
        }

        private Impostos ImpostosFiltrados
        {
            get
            {
                var lista = new List<Imposto>();
                for (int i = 0; i < pvtImpostos.Items.Count; i++)
                {
                    var contexto = ((pvtImpostos.Items[i] as PivotItem).Content as FrameworkElement).DataContext;
                    if (contexto is Imposto)
                        lista.Add(contexto as Imposto);
                    else if (contexto is IImpostoDataContext)
                        lista.Add((contexto as IImpostoDataContext).ImpostoBruto);
                    else if (contexto is IImpostosUnidos)
                        lista.AddRange((contexto as IImpostosUnidos).SepararImpostos());
                }
                return new Impostos(from i in lista
                                    where i.IsValido
                                    select i);
            }
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            var data = DataContext as ProdutoCompletoDataContext;
            data.ProdutoCompleto.impostos = ImpostosFiltrados;
            CoreApplication.Properties.Add("ProdutoPendente", data.ProdutoCompleto);
            Propriedades.Intercambio.Retornar();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Propriedades.Intercambio.Retornar();
        }
    }
}
