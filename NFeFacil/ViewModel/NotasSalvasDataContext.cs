using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.Repositorio;
using NFeFacil.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;

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

        public NotasSalvasDataContext(ref ListView lista)
        {
            lista.SelectionChanged += (x, y) =>
            {
                ItensSelecionados = (x as ListView).SelectedItems;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExibirEditar)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExibirRemoverSelecionados)));
            };
        }

        private async static void Editar(NFeDI nota)
        {
            var conjunto = new ConjuntoManipuladorNFe
            {
                StatusAtual = (StatusNFe)nota.Status,
                OperacaoRequirida = TipoOperacao.Edicao
            };
            if (nota.Status < 4)
            {
                conjunto.NotaSalva = (await nota.ObjetoCompletoAsync()) as NFe;
            }
            else
            {
                conjunto.NotaEmitida = (await nota.ObjetoCompletoAsync()) as Processo;
            }
            (Window.Current.Content as Frame)
                .Navigate(typeof(ManipulacaoNotaFiscal), conjunto, new DrillInNavigationTransitionInfo());
        }

        private async void Remover(NFeDI nota)
        {
            using (var db = new NotasFiscais())
            {
                await db.Remover(nota);
                db.SalvarMudancas();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotasSalvas)));
            }
        }

        private async void RemoverSelecionados()
        {
            using (var db = new NotasFiscais())
            {
                for (int i = 0; i < ItensSelecionados.Count; i++)
                {
                    await db.Remover(ItensSelecionados[i] as NFeDI);
                }
                db.SalvarMudancas();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotasSalvas)));
            }
        }
    }
}
