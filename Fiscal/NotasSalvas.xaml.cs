using BaseGeral.ItensBD;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BaseGeral.Controles;
using System.Xml.Serialization;
using NFeFacil.View;
using NFeFacil.WebService.Pacotes.PartesEnvEvento;
using NFeFacil.WebService.Pacotes.PartesRetEnvEvento;
using Windows.UI.Xaml.Navigation;
using BaseGeral;
using BaseGeral.View;
using Fiscal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal
{
    [DetalhePagina(Symbol.Library, "Notas salvas")]
    public sealed partial class NotasSalvas : Page, IHambuguer
    {
        IControleView Controle;

        public NotasSalvas()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Controle = (IControleView)e.Parameter;
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                var (emitidas, outras, canceladas) = repo.ObterNotas(
                    DefinicoesTemporarias.EmitenteAtivo.CNPJ, Controle.IsNFCe);
                NotasEmitidas = emitidas.GerarObs();
                OutrasNotas = outras.GerarObs();
                NotasCanceladas = canceladas.GerarObs();
            }
        }

        ObservableCollection<NFeDI> NotasEmitidas { get; set; }
        ObservableCollection<NFeDI> OutrasNotas { get; set; }
        ObservableCollection<NFeDI> NotasCanceladas { get; set; }

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Send, "Emitidas"),
            new ItemHambuguer(Symbol.SaveLocal, "Outras"),
            new ItemHambuguer(Symbol.Cancel, "Canceladas")
        };

        void Exibir(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            Controle.Exibir(nota);
        }

        void Excluir(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            using (var repo = new BaseGeral.Repositorio.OperacoesExtras())
            {
                repo.ExcluirNFe(nota);
                OutrasNotas.Remove(nota);
            }
        }

        async void Cancelar(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            if (await Controle.Cancelar(nota))
            {
                NotasEmitidas.Remove(nota);
                NotasCanceladas.Insert(0, nota);
            }
        }

        public int SelectedIndex { set => main.SelectedIndex = value; }

        [XmlRoot("procEventoNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public struct ProcEventoCancelamento
        {
            [XmlAttribute("versao")]
            public string Versao { get; set; }

            [XmlElement("evento")]
            public Evento[] Eventos { get; set; }

            [XmlElement("retEvento")]
            public ResultadoEvento[] RetEvento { get; set; }
        }

        async void CriarCopia(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            await Controle.CriarCopia(nota);
        }
    }
}
