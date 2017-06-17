using BibliotecaCentral;
using BibliotecaCentral.IBGE;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral.WebService;
using BibliotecaCentral.WebService.Pacotes;
using NFeFacil.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel
{
    public sealed class NotasSalvasDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IList<object> ItensSelecionados { get; set; }

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

        public bool ExibirEditar => (ItensSelecionados?.Count ?? 0) <= 1;
        public bool ExibirRemoverSelecionados => (ItensSelecionados?.Count ?? 0) > 1;

        public ICommand EditarCommand { get; } = new Comando<NFeDI>(Editar);
        public ICommand RemoverCommand => new Comando<NFeDI>(Remover);
        public ICommand RemoverSelecionadosCommand => new Comando(RemoverSelecionados, true);
        public ICommand CancelarCommand => new Comando<NFeDI>(Cancelar);

        public NotasSalvasDataContext(ref ListView lista)
        {
            lista.SelectionChanged += (x, y) =>
            {
                ItensSelecionados = (x as ListView).SelectedItems;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExibirEditar)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExibirRemoverSelecionados)));
            };
        }

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

        private void RemoverSelecionados()
        {
            using (var db = new NotasFiscais())
            {
                for (int i = 0; i < ItensSelecionados.Count; i++)
                {
                    db.Remover(ItensSelecionados[i] as NFeDI);
                }
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
            }
        }
    }
}
