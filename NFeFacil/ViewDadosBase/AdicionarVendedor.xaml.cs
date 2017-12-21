using NFeFacil.ItensBD;
using NFeFacil.Validacao;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [View.DetalhePagina(Symbol.People, "Vendedor")]
    public sealed partial class AdicionarVendedor : Page
    {
        private Vendedor Vendedor { get; set; }

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
                if (new ValidarDados().ValidarTudo(true,
                    (string.IsNullOrWhiteSpace(Vendedor.CPFStr), "CPF inválido"),
                    (string.IsNullOrWhiteSpace(Vendedor.Nome), "Nome não pode estar em branco"),
                    (string.IsNullOrWhiteSpace(Vendedor.Endereço), "Endereço não pode estar em branco")))
                {
                    using (var repo = new Repositorio.Escrita())
                    {
                        repo.SalvarItemSimples(Vendedor, Propriedades.DateTimeNow);
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
