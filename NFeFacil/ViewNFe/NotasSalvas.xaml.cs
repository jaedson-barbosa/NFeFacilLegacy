using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using System.Collections;
using System.Collections.Generic;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
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

        public event EventHandler MainMudou;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.Current.SeAtualizar(Symbol.Library, "Notas salvas");
        }

        ObservableCollection<NFeView> NotasEmitidas { get; }
        ObservableCollection<NFeView> OutrasNotas { get; }

        ICollectionView NotasEmitidasView => new CollectionViewSource() { Source = NotasEmitidas }.View;
        ICollectionView OutrasNotasView => new CollectionViewSource() { Source = OutrasNotas }.View;

        public IEnumerable ConteudoMenu
        {
            get
            {
                var retorno = new ObservableCollection<Controles.ItemHambuguer>
                {
                    new Controles.ItemHambuguer(Symbol.Send, "Emitidas"),
                    new Controles.ItemHambuguer(Symbol.SaveLocal, "Outras")
                };
                main.SelectionChanged += (sender, e) => MainMudou?.Invoke(this, new NewIndexEventArgs { NewIndex = main.SelectedIndex });
                return retorno;
            }
        }

        private void Remover(object sender, RoutedEventArgs e)
        {
            var nota = (NFeView)((MenuFlyoutItem)sender).DataContext;
            var Nota = nota.Nota;
            using (var db = new AplicativoContext())
            {
                db.Remove(Nota);
                db.SaveChanges();
            }

            if (Nota.Status == (int)StatusNFe.Emitida)
            {
                NotasEmitidas.RemoveAt(NotasEmitidas.IndexOf(nota));
            }
            else
            {
                OutrasNotas.RemoveAt(OutrasNotas.IndexOf(nota));
            }
        }

        private void Editar(object sender, RoutedEventArgs e)
        {
            var nota = (NFeView)((MenuFlyoutItem)sender).DataContext;
            var Nota = nota.Nota;
            var conjunto = new ConjuntoManipuladorNFe
            {
                StatusAtual = (StatusNFe)nota.Nota.Status,
                Impressa = nota.Nota.Impressa,
                Exportada = nota.Nota.Exportada,
            };

            if (Nota.Status < 4)
            {
                conjunto.NotaSalva = XElement.Parse(Nota.XML).FromXElement<NFe>();
            }
            else
            {
                conjunto.NotaEmitida = XElement.Parse(Nota.XML).FromXElement<Processo>();
            }
            MainPage.Current.Navegar<ViewNFe.VisualizacaoNFe>(conjunto.NotaSalva ?? conjunto.NotaEmitida.NFe);
        }

        private async void Cancelar(object sender, RoutedEventArgs e)
        {
            var nota = (NFeView)((MenuFlyoutItem)sender).DataContext;
            var Nota = nota.Nota;
            var processo = XElement.Parse(Nota.XML).FromXElement<Processo>();
            if (/*await new OperacoesNotaEmitida(processo).Cancelar()*/true)
            {
                Nota.Status = (int)StatusNFe.Cancelada;
                //using (var db = new AplicativoContext())
                //{
                //    Nota.UltimaData = DateTime.Now;
                //    db.Update(Nota);
                //    db.SaveChanges();
                //}

                nota.CalcularMensagemApoio();
                if (Nota.Status == (int)StatusNFe.Emitida)
                {
                    NotasEmitidas[NotasEmitidas.IndexOf(nota)] = nota;
                }
                else
                {
                    OutrasNotas[OutrasNotas.IndexOf(nota)] = nota;
                }
            }
        }

        public void AtualizarMain(int index) => main.SelectedIndex = index;

        sealed class NFeView
        {
            public NFeDI Nota { get; }
            public string MensagemApoio { get; set; }
            public bool PodeCancelar => Nota.Status == (int)StatusNFe.Emitida;

            public NFeView(NFeDI nota)
            {
                Nota = nota;
                CalcularMensagemApoio();
            }

            public void CalcularMensagemApoio()
            {
                if (PodeCancelar)
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
    }
}
