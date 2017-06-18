using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.Repositorio;
using NFeFacil.View;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Xaml.Data;

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
                                 let item = new NFeView(nota, this)
                                 group item by item.Status;
                    return new CollectionViewSource()
                    {
                        IsSourceGrouped = true,
                        Source = source
                    }.View;
                }
            }
        }

        public void Editar(NFeDI nota)
        {
            var conjunto = new ConjuntoManipuladorNFe
            {
                StatusAtual = (StatusNFe)nota.Status,
                Impressa = nota.Impressa,
                Exportada = nota.Exportada,
                OperacaoRequirida = TipoOperacao.Edicao
            };
            if (nota.Status < 4)
            {
                conjunto.NotaSalva = XElement.Parse(nota.XML).FromXElement<NFe>();
            }
            else
            {
                conjunto.NotaEmitida = XElement.Parse(nota.XML).FromXElement<Processo>();
            }
            MainPage.Current.AbrirFunçao(typeof(ManipulacaoNotaFiscal), conjunto);
        }

        public void Remover(NFeDI nota)
        {
            using (var db = new NotasFiscais())
            {
                db.Remover(nota);
                db.SalvarMudancas();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotasSalvas)));
            }
        }

        public async void Cancelar(NFeDI di)
        {
            var processo = XElement.Parse(di.XML).FromXElement<Processo>();
            if (await new OperacoesNotaEmitida(processo).Cancelar())
            {
                di.Status = (int)StatusNFe.Cancelada;
                using (var db = new NotasFiscais())
                {
                    db.Atualizar(di);
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotasSalvas)));
            }
        }

        sealed class NFeView
        {
            public NFeDI Nota { get; }

            public StatusNFe Status { get; set; }

            public bool PodeCancelar => Status == StatusNFe.Emitida;

            public ICommand EditarCommand { get; }
            public ICommand RemoverCommand { get; }
            public ICommand CancelarCommand { get; }

            public NFeView(NFeDI nota, NotasSalvasDataContext geral)
            {
                Nota = nota;
                Status = (StatusNFe)nota.Status;
                
                EditarCommand = new Comando<NFeDI>(geral.Editar);
                RemoverCommand = new Comando<NFeDI>(geral.Remover);
                CancelarCommand = new Comando<NFeDI>(geral.Cancelar);
            }

            public static implicit operator NFeDI(NFeView nota)
            {
                return nota.Nota;
            }
        }
    }
}
