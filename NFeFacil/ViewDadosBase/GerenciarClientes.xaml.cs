using NFeFacil.ItensBD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [View.DetalhePagina(Symbol.Manage, "Gerenciar clientes")]
    public sealed partial class GerenciarClientes : Page
    {
        ClienteDI[] TodosClientes { get; }
        ObservableCollection<ClienteDI> Clientes { get; }

        public GerenciarClientes()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                TodosClientes = repo.ObterClientes().ToArray();
                Clientes = TodosClientes.GerarObs();
            }
        }

        async void AdicionarCliente(object sender, RoutedEventArgs e)
        {
            var caixa = new EscolherTipoCliente();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                switch (caixa.TipoCliente)
                {
                    case 0:
                        MainPage.Current.Navegar<AdicionarClienteBrasileiroPF>();
                        break;
                    case 1:
                        MainPage.Current.Navegar<AdicionarClienteBrasileiroPFContribuinte>();
                        break;
                    case 2:
                        MainPage.Current.Navegar<AdicionarClienteBrasileiroPJ>();
                        break;
                    case 3:
                        MainPage.Current.Navegar<AdicionarClienteEstrangeiro>();
                        break;
                }
            }
        }

        private void EditarCliente(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var dest = (ClienteDI)contexto;

            if (!string.IsNullOrEmpty(dest.CPF))
            {
                if (dest.IndicadorIE == 1)
                {
                    MainPage.Current.Navegar<AdicionarClienteBrasileiroPFContribuinte>(dest);
                }
                else
                {
                    MainPage.Current.Navegar<AdicionarClienteBrasileiroPF>(dest);
                }
            }
            else if (!string.IsNullOrEmpty(dest.CNPJ))
            {
                MainPage.Current.Navegar<AdicionarClienteBrasileiroPJ>(dest);
            }
            else
            {
                MainPage.Current.Navegar<AdicionarClienteEstrangeiro>(dest);
            }
        }

        private void InativarCliente(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var dest = (ClienteDI)contexto;

            using (var repo = new Repositorio.Escrita())
            {
                repo.InativarDadoBase(dest, DefinicoesTemporarias.DateTimeNow);
                Clientes.Remove(dest);
            }
        }

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < TodosClientes.Length; i++)
            {
                var atual = TodosClientes[i];
                bool valido = (DefinicoesPermanentes.ModoBuscaCliente == 0
                    ? atual.Nome : atual.Documento).ToUpper().Contains(busca.ToUpper());
                if (valido && !Clientes.Contains(atual))
                {
                    Clientes.Add(atual);
                }
                else if (!valido && Clientes.Contains(atual))
                {
                    Clientes.Remove(atual);
                }
            }
        }
    }
}
