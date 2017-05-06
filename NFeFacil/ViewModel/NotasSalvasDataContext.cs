using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.Repositorio;
using NFeFacil.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewModel
{
    public sealed class NotasSalvasDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IList<object> ItensSelecionados { get; set; }
        public ObservableCollection<NFeDI> NotasSalvas { get; set; }

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
            using (var db = new NotasFiscais())
            {
                NotasSalvas = db.Registro.GerarObs();
            }
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
                conjunto.NotaSalva = (await nota.ConjuntoCompletoAsync()) as NFe;
            }
            else
            {
                conjunto.NotaEmitida = (await nota.ConjuntoCompletoAsync()) as Processo;
            }
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ManipulacaoNotaFiscal), conjunto);
        }

        private async void Remover(NFeDI nota)
        {
            using (var db = new NotasFiscais())
            {
                await db.Remover(nota);
                db.SalvarMudancas();
                NotasSalvas = db.Registro.GerarObs();
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
                NotasSalvas = db.Registro.GerarObs();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotasSalvas)));
            }
        }
    }
}
