using System;
using NFeFacil.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoNotaFiscal : Page, IHambuguer
    {
        public ManipulacaoNotaFiscal()
        {
            InitializeComponent();
        }

        public ListView ConteudoMenu
        {
            get
            {
                var lista = new ListView()
                {
                    ItemsSource = new ObservableCollection<Controles.ItemHambuguer>
                    {
                        new Controles.ItemHambuguer(Symbol.Tag, "Identificação"),
                        new Controles.ItemHambuguer(Symbol.People, "Cliente"),
                        new Controles.ItemHambuguer(Symbol.Street, "Retirada/Entrega"),
                        new Controles.ItemHambuguer(Symbol.Shop, "Produtos"),
                        new Controles.ItemHambuguer(Symbol.Calculator, "Totais"),
                        new Controles.ItemHambuguer("\uE806", "Transporte"),
                        new Controles.ItemHambuguer("\uE825", "Cobrança"),
                        new Controles.ItemHambuguer(Symbol.Comment, "Informações adicionais"),
                        new Controles.ItemHambuguer(Symbol.World, "Exportação e compras"),
                        new Controles.ItemHambuguer(new Uri("ms-appx:///Assets/CanaAcucar.png"), "Cana-de-açúcar")
                    },
                    SelectedIndex = 0
                };
                pvtPrincipal.SelectionChanged += (sender, e) => lista.SelectedIndex = pvtPrincipal.SelectedIndex;
                lista.SelectionChanged += (sender, e) => pvtPrincipal.SelectedIndex = lista.SelectedIndex;
                return lista;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = (ConjuntoManipuladorNFe)e.Parameter;
            switch (param.OperacaoRequirida)
            {
                case TipoOperacao.Adicao:
                    MainPage.Current.SeAtualizar(Symbol.Add, "Nota fiscal");
                    break;
                case TipoOperacao.Edicao:
                    MainPage.Current.SeAtualizar(Symbol.Edit, "Nota fiscal");
                    break;
            }
            DataContext = new NotaFiscalDataContext(ref param);
        }
    }
}
