using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

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
            using (var repo = new Repositorio.MEGACLASSE())
            {
                Vendas = repo.ObterRegistrosVenda(Propriedades.EmitenteAtivo.Id).Select(x => new ExibicaoVenda
                {
                    Base = x.rv,
                    NomeCliente = x.cliente,
                    NomeVendedor = x.vendedor,
                    DataHoraVenda = x.momento
                }).GerarObs();
            }
        }

        private void Exibir(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var item = (MenuFlyoutItem)sender;
            var venda = (ExibicaoVenda)item.DataContext;
            MainPage.Current.Navegar<VisualizacaoRegistroVenda>(venda.Base);
        }
    }

    public struct ExibicaoVenda
    {
        public RegistroVenda Base { get; set; }
        public string NomeVendedor { get; set; }
        public string NomeCliente { get; set; }
        public string DataHoraVenda { get; set; }
    }
}
