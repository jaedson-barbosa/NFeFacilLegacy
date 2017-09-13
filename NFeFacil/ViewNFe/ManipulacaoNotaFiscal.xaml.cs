using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Collections;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.View.Controles;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
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

        public IEnumerable ConteudoMenu
        {
            get
            {
                var retorno = new ObservableCollection<ItemHambuguer>
                {
                    new ItemHambuguer(Symbol.Tag, "Identificação"),
                    new ItemHambuguer(Symbol.People, "Cliente"),
                    new ItemHambuguer(Symbol.Street, "Retirada/Entrega"),
                    new ItemHambuguer(Symbol.Shop, "Produtos"),
                    new ItemHambuguer(Symbol.Calculator, "Totais"),
                    new ItemHambuguer("\uE806", "Transporte"),
                    new ItemHambuguer("\uE825", "Cobrança"),
                    new ItemHambuguer(Symbol.Comment, "Informações adicionais"),
                    new ItemHambuguer(Symbol.World, "Exportação e compras"),
                    new ItemHambuguer(new Uri("ms-appx:///Assets/CanaAcucar.png"), "Cana-de-açúcar")
                };
                pvtPrincipal.SelectionChanged += (sender, e) => MainMudou?.Invoke(this, new NewIndexEventArgs { NewIndex = pvtPrincipal.SelectedIndex });
                return retorno;
            }
        }

        public event EventHandler MainMudou;
        public void AtualizarMain(int index) => pvtPrincipal.SelectedIndex = index;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = (NFe)e.Parameter;
            if (string.IsNullOrEmpty(param.Informações.Id))
            {
                MainPage.Current.SeAtualizar(Symbol.Add, "Nota fiscal");
            }
            else
            {
                MainPage.Current.SeAtualizar(Symbol.Edit, "Nota fiscal");
            }
            DataContext = new NotaFiscalDataContext(ref param);
        }
    }
}
