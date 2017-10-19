using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using NFeFacil.View.Controles;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class NotasSalvas : Page, IHambuguer
    {
        public NotasSalvas()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                var notasFiscais = db.NotasFiscais.ToArray();
                NotasEmitidas = (from nota in notasFiscais
                                 where nota.Status == (int)StatusNFe.Emitida
                                 orderby nota.DataEmissao descending
                                 select new NFeView(nota)).GerarObs();
                OutrasNotas = (from nota in notasFiscais
                               where nota.Status != (int)StatusNFe.Emitida
                               orderby nota.DataEmissao descending
                               select new NFeView(nota)).GerarObs();
                
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Library, "Notas salvas");
        }

        ObservableCollection<NFeView> NotasEmitidas { get; }
        ObservableCollection<NFeView> OutrasNotas { get; }

        ICollectionView NotasEmitidasView => new CollectionViewSource() { Source = NotasEmitidas }.View;
        ICollectionView OutrasNotasView => new CollectionViewSource() { Source = OutrasNotas }.View;

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Send, "Emitidas"),
            new ItemHambuguer(Symbol.SaveLocal, "Outras")
        };

        private void Exibir(object sender, RoutedEventArgs e)
        {
            var nota = (NFeView)((MenuFlyoutItem)sender).DataContext;
            MainPage.Current.Navegar<VisualizacaoNFe>(nota.Nota);
        }

        private async void Cancelar(object sender, RoutedEventArgs e)
        {
            var nota = (NFeView)((MenuFlyoutItem)sender).DataContext;
            var Nota = nota.Nota;
            var processo = XElement.Parse(Nota.XML).FromXElement<Processo>();
            if (await new OperacoesNotaEmitida(processo).Cancelar())
            {
                Nota.Status = (int)StatusNFe.Cancelada;
                using (var db = new AplicativoContext())
                {
                    Nota.UltimaData = DateTime.Now;
                    db.Update(Nota);
                    db.SaveChanges();
                }

                nota.CalcularMensagemApoio();
                NotasEmitidas.Remove(nota);
                OutrasNotas.Add(nota);
            }
        }

        public void AtualizarMain(int index) => main.SelectedIndex = index;

        sealed class NFeView
        {
            public NFeDI Nota { get; }
            public string MensagemApoio { get; set; }

            public NFeView(NFeDI nota)
            {
                Nota = nota;
                CalcularMensagemApoio();
            }

            public void CalcularMensagemApoio()
            {
                if (Nota.Status == (int)StatusNFe.Emitida)
                {
                    MensagemApoio = $"Exportada: {BoolToString(Nota.Exportada)}; Impressa: {BoolToString(Nota.Impressa)}";
                }
                else
                {
                    MensagemApoio = $"Status: {((StatusNFe)Nota.Status).ToString()}";
                }

                string BoolToString(bool booleano) => booleano ? "Sim" : "Não";
            }

            public override bool Equals(object obj)
            {
                if (obj is NFeView view)
                {
                    return Nota.Id == view.Nota.Id;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Nota.Id.GetHashCode();
            }
        }

        private void TelaMudou(object sender, SelectionChangedEventArgs e)
        {
            var index = ((FlipView)sender).SelectedIndex;
            MainPage.Current.AlterarSelectedIndexHamburguer(index);
        }
    }
}
