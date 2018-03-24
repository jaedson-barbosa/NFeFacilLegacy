using BaseGeral;
using BaseGeral.ItensBD;
using NFeFacil.View;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    [DetalhePagina(Symbol.Home, "Dados da empresa")]
    public sealed partial class GeralEmitente : Page
    {
        EmitenteDI emitente;
        ImageSource imagem;

        public GeralEmitente()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var emitente = (ConjuntoBasicoExibicao<EmitenteDI>)e.Parameter;
            var brush = new ImageBrush { ImageSource = emitente.Imagem };
            imgLogotipo.Background = brush;
            txtNomeFantasia.Text = emitente.Principal;
            txtNome.Text = emitente.Secundario;
            this.emitente = emitente.Objeto;
            imagem = emitente.Imagem;
        }

        private void Confirmar(object sender, RoutedEventArgs e)
        {
            DefinicoesTemporarias.EmitenteAtivo = emitente;
            DefinicoesTemporarias.Logotipo = imagem;
            MainPage.Current.Navegar<EscolhaVendedor>();
        }

        private void Editar(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarEmitente>(emitente);
        }

        async void Logotipo(object sender, RoutedEventArgs e)
        {
            var brush = (ImageBrush)imgLogotipo.Background;
            var caixa = new View.DefinirImagem(emitente.Id, brush.ImageSource);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                brush.ImageSource = caixa.Imagem;
                imagem = caixa.Imagem;
            }
        }
    }
}
