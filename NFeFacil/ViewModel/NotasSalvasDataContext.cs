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
                using (var db = new NotasFiscais())
                {
                    return new CollectionViewSource()
                    {
                        IsSourceGrouped = true,
                        Source = from nota in db.Registro
                                 group nota by ((StatusNFe)nota.Status).ToString()
                    }.View;
                }
            }
        }

        public ICommand EditarCommand { get; } = new Comando<NFeDI>(Editar);
        public ICommand RemoverCommand => new Comando<NFeDI>(Remover);
        public ICommand CancelarCommand => new Comando<NFeDI>(Cancelar);

        private static void Editar(NFeDI nota)
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

        private void Remover(NFeDI nota)
        {
            using (var db = new NotasFiscais())
            {
                db.Remover(nota);
                db.SalvarMudancas();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotasSalvas)));
            }
        }

        async void Cancelar(NFeDI di)
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
    }
}
