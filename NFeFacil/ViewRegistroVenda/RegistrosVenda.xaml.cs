using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class RegistrosVenda : Page
    {
        ObservableCollection<ExibicaoVenda> Vendas { get; }

        public RegistrosVenda()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                Vendas = (from venda in db.Vendas.Include(x => x.Produtos).ToArray()
                          where venda.Emitente == Propriedades.EmitenteAtivo.Id
                          orderby venda.DataHoraVenda descending
                          select new ExibicaoVenda
                          {
                              Base = venda,
                              NomeVendedor = venda.Vendedor != default(Guid) ? db.Vendedores.Find(venda.Vendedor).Nome : "Indisponível",
                              NomeCliente = venda.Cliente != default(Guid) ? db.Clientes.Find(venda.Cliente).Nome : "Indisponível",
                              DataHoraVenda = venda.DataHoraVenda.ToString("HH:mm:ss dd-MM-yyyy")
                          }).GerarObs();
            }
        }

        public struct ExibicaoVenda
        {
            public RegistroVenda Base { get; set; }
            public string NomeVendedor { get; set; }
            public string NomeCliente { get; set; }
            public string DataHoraVenda { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Library, "Vendas");
        }

        private void Exibir(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var item = (MenuFlyoutItem)sender;
            var venda = (ExibicaoVenda)item.DataContext;
            MainPage.Current.Navegar<VisualizacaoRegistroVenda>(venda.Base);
        }
    }
}
