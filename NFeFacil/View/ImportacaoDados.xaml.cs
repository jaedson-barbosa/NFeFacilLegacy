using NFeFacil.Importacao;
using NFeFacil.Log;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ImportacaoDados : Page
    {
        public ImportacaoDados()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Setting, "Configurações");
        }

        private void ImportarCliente(object sender, TappedRoutedEventArgs e)
        {
            ImportarDadoBase(TiposDadoBasico.Cliente);
        }

        private void ImportarMotorista(object sender, TappedRoutedEventArgs e)
        {
            ImportarDadoBase(TiposDadoBasico.Motorista);
        }

        private void ImportarProduto(object sender, TappedRoutedEventArgs e)
        {
            ImportarDadoBase(TiposDadoBasico.Produto);
        }

        private void ImportarNotaFiscal(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ImportarNotaFiscal();
        }

        private async void ImportarDadoBase(TiposDadoBasico tipo)
        {
            var resultado = await new ImportarDadoBase(tipo).ImportarAsync();
            if (resultado.Count == 0)
            {
                Popup.Current.Escrever(TitulosComuns.Sucesso, "As informações base foram importadas com sucesso.");
            }
            else
            {
                StringBuilder stringErros = new StringBuilder();
                stringErros.AppendLine("Os seguintes dados base não foram reconhecidos por terem a tag raiz diferente do esperado.");
                resultado.ForEach(y =>
                {
                    if (y is XmlNaoReconhecido x)
                    {
                        stringErros.AppendLine($"Nome arquivo: {x.NomeArquivo}; Tag raiz encontrada: {x.TagRaiz}; Tags raiz esperadas: {x.TagsEsperadas[0]} ou {x.TagsEsperadas[1]}");
                    }
                    else
                    {
                        stringErros.AppendLine($"Mensagem erro: {y.Message}.");
                    }
                });
                Popup.Current.Escrever(TitulosComuns.Erro, stringErros.ToString());
            }
        }

        private async void ImportarNotaFiscal()
        {
            var resultado = await new ImportarNotaFiscal().ImportarAsync();
            if (resultado.Count == 0)
            {
                Popup.Current.Escrever(TitulosComuns.Sucesso, "As notas fiscais foram importadas com sucesso.");
            }
            else
            {
                StringBuilder stringErros = new StringBuilder();
                stringErros.AppendLine("As seguintes notas fiscais não foram reconhecidas por terem a tag raiz diferente de nfeProc e de NFe.");
                resultado.ForEach(y =>
                {
                    if (y is XmlNaoReconhecido x)
                    {
                        stringErros.AppendLine($"Nome arquivo: {x.NomeArquivo}; Tag raiz: Encontrada: {x.TagRaiz}");
                    }
                    else
                    {
                        stringErros.AppendLine($"Mensagem erro: {y.Message}.");
                    }
                });
                Popup.Current.Escrever(TitulosComuns.Erro, stringErros.ToString());
            }
        }
    }
}
