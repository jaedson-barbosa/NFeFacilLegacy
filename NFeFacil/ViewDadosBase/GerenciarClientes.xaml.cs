using NFeFacil.ItensBD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarClientes : Page
    {
        ObservableCollection<ClienteDI> Clientes { get; }

        public GerenciarClientes()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
                Clientes = db.Clientes.Where(x => x.Ativo).OrderBy(x => x.Nome).GerarObs();
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

            using (var db = new AplicativoContext())
            {
                dest.Ativo = false;
                db.Update(dest);
                db.SaveChanges();
                Clientes.Remove(dest);
            }
        }
    }
}
