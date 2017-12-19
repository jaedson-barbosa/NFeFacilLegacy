using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.Validacao;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarVendedor : Page
    {
        private Vendedor Vendedor { get; set; }
        private ILog Log = Popup.Current;

        string Endereco
        {
            get => Vendedor.Endereço;
            set => Vendedor.Endereço = value;
        }

        public AdicionarVendedor()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                Vendedor = new Vendedor();
            }
            else
            {
                Vendedor = (Vendedor)e.Parameter;
            }
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorVendedor(Vendedor).Validar(Log))
                {
                    using (var repo = new Repositorio.MEGACLASSE())
                    {
                        repo.SalvarVendedor(Vendedor, Propriedades.DateTimeNow);
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
