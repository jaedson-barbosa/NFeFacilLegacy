using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.Validacao;
using BaseGeral.View;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [DetalhePagina(Symbol.People, "Comprador")]
    public sealed partial class AdicionarComprador : Page
    {
        Comprador Comprador;
        ObservableCollection<ClienteDI> ClientesDisponiveis { get; }

        public AdicionarComprador()
        {
            InitializeComponent();
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                ClientesDisponiveis = repo.ObterClientes(x => !string.IsNullOrEmpty(x.CNPJ)).GerarObs();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                Comprador = new Comprador();
            }
            else
            {
                Comprador = (Comprador)e.Parameter;
            }
            DataContext = Comprador;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidarDados().ValidarTudo(true,
                    (Comprador.IdEmpresa == default(Guid), "Selecione uma empresa 'dona' deste comprador"),
                    (string.IsNullOrEmpty(Comprador.Telefone), "Telefone não pode estar em branco"),
                    (string.IsNullOrWhiteSpace(Comprador.Nome), "Nome não pode estar em branco"),
                    (string.IsNullOrWhiteSpace(Comprador.Email), "Email não pode estar em branco")))
                {
                    using (var repo = new BaseGeral.Repositorio.Escrita())
                    {
                        repo.SalvarItemSimples(Comprador, DefinicoesTemporarias.DateTimeNow);
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }
    }
}
