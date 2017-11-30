using NFeFacil.ItensBD;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GeralEmitente : Page
    {
        EmitenteDI emitente;

        public GeralEmitente()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var emitente = (ConjuntoBasicoExibicao)e.Parameter;
            var brush = new ImageBrush
            {
                ImageSource = emitente.Imagem
            };
            imgLogotipo.Background = brush;
            txtNomeFantasia.Text = emitente.Principal;
            txtNome.Text = emitente.Secundario;
            using (var db = new AplicativoContext())
            {
                this.emitente = db.Emitentes.Find(emitente.Id);
            }
            MainPage.Current.SeAtualizar(Symbol.Home, "Dados da empresa");
        }

        private void Confirmar(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Propriedades.EmitenteAtivo = emitente;
            MainPage.Current.Navegar<EscolhaVendedor>();
        }

        private void Editar(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarEmitente>(emitente);
        }

        async void Logotipo(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var brush = (ImageBrush)imgLogotipo.Background;
            var caixa = new View.DefinirImagem(emitente.Id, brush.ImageSource);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                brush.ImageSource = caixa.Imagem;
            }
        }
    }
}
