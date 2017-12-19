using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.Validacao;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarComprador : Page
    {
        Comprador Comprador;
        ILog Log = Popup.Current;
        ObservableCollection<ClienteDI> ClientesDisponiveis { get; }

        public AdicionarComprador()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
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
                if (new ValidadorComprador(Comprador).Validar(Log))
                {
                    using (var repo = new Repositorio.Escrita())
                    {
                        repo.SalvarComprador(Comprador, Propriedades.DateTimeNow);
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
