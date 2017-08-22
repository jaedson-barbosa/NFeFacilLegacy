using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.View;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace NFeFacil.ViewModel
{
    public sealed class NotasSalvasDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICollectionView NotasSalvas
        {
            get
            {
                using (var db = new AplicativoContext())
                {
                    var source = from nota in db.NotasFiscais
                                 orderby nota.DataEmissao descending
                                 let item = new NFeView(nota, AttNotasSalvas)
                                 group item by item.Status;
                    return new CollectionViewSource()
                    {
                        IsSourceGrouped = true,
                        Source = source
                    }.View;
                }
            }
        }

        void AttNotasSalvas()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotasSalvas)));
        }

        sealed class NFeView
        {
            public NFeDI Nota { get; }
            public StatusNFe Status { get; set; }
            public bool PodeCancelar => Status == StatusNFe.Emitida;

            Action AtualizarNotasSalvas { get; }
            public ICommand EditarCommand { get; }
            public ICommand RemoverCommand { get; }
            public ICommand CancelarCommand { get; }

            public Brush Background { get; }

            public NFeView(NFeDI nota, Action attNotasSalvas)
            {
                Nota = nota;
                Status = (StatusNFe)nota.Status;
                AtualizarNotasSalvas = attNotasSalvas;

                EditarCommand = new Comando(Editar);
                RemoverCommand = new Comando(Remover);
                CancelarCommand = new Comando(Cancelar);

                Background = DefinirBackground();
            }

            Brush DefinirBackground()
            {
                Color cor;
                if (Nota.Exportada && Nota.Impressa)
                {
                    cor = Colors.Green;
                }
                else if (Nota.Exportada)
                {
                    cor = Colors.Blue;
                }
                else if (Nota.Impressa)
                {
                    cor = Colors.Yellow;
                }
                else
                {
                    cor = Colors.Red;
                }
                cor.A = 100;
                return new SolidColorBrush(cor);
            }

            public void Editar()
            {
                var conjunto = new ConjuntoManipuladorNFe
                {
                    StatusAtual = (StatusNFe)Nota.Status,
                    Impressa = Nota.Impressa,
                    Exportada = Nota.Exportada,
                    OperacaoRequirida = TipoOperacao.Edicao
                };
                if (Nota.Status < 4)
                {
                    conjunto.NotaSalva = XElement.Parse(Nota.XML).FromXElement<NFe>();
                }
                else
                {
                    conjunto.NotaEmitida = XElement.Parse(Nota.XML).FromXElement<Processo>();
                }
                MainPage.Current.Navegar<ManipulacaoNotaFiscal>(conjunto);
            }

            public void Remover()
            {
                using (var db = new AplicativoContext())
                {
                    db.Remove(Nota);
                    db.SaveChanges();
                    AtualizarNotasSalvas();
                }
            }

            public async void Cancelar()
            {
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
                    AtualizarNotasSalvas();
                }
            }
        }
    }
}
