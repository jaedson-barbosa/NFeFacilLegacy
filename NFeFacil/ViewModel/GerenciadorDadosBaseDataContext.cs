using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using BibliotecaCentral.Repositorio;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public sealed class GerenciadorDadosBaseDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nome)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
        }

        public ObservableCollection<EmitenteDI> Emitentes { get; private set; }
        public ObservableCollection<ClienteDI> Clientes { get; private set; }
        public ObservableCollection<Motorista> Motoristas { get; private set; }
        public ObservableCollection<BaseProdutoOuServico> Produtos { get; private set; }

        public ICommand AdicionarEmitenteCommand { get; }
        public ICommand EditarEmitenteCommand { get; }
        public ICommand RemoverEmitenteCommand { get; }

        public ICommand AdicionarClienteCommand { get; }
        public ICommand EditarClienteCommand { get; }
        public ICommand RemoverClienteCommand { get; }

        public ICommand AdicionarMotoristaCommand { get; }
        public ICommand EditarMotoristaCommand { get; }
        public ICommand RemoverMotoristaCommand { get; }

        public ICommand AdicionarProdutoCommand { get; }
        public ICommand EditarProdutoCommand { get; }
        public ICommand RemoverProdutoCommand { get; }

        public GerenciadorDadosBaseDataContext()
        {
            AdicionarEmitenteCommand = new Comando(AdicionarEmitente);
            EditarEmitenteCommand = new Comando<EmitenteDI>(EditarEmitente);
            RemoverEmitenteCommand = new Comando<EmitenteDI>(RemoverEmitente);

            AdicionarClienteCommand = new Comando(AdicionarCliente);
            EditarClienteCommand = new Comando<ClienteDI>(EditarCliente);
            RemoverClienteCommand = new Comando<ClienteDI>(RemoverCliente);

            AdicionarMotoristaCommand = new Comando(AdicionarMotorista);
            EditarMotoristaCommand = new Comando<Motorista>(EditarMotorista);
            RemoverMotoristaCommand = new Comando<Motorista>(RemoverMotorista);

            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            EditarProdutoCommand = new Comando<BaseProdutoOuServico>(EditarProduto);
            RemoverProdutoCommand = new Comando<BaseProdutoOuServico>(RemoverProduto);

            DefinirAsync();

            async void DefinirAsync()
            {
                await DefinirEmitentesAsync();
                await DefinirClientesAsync();
                await DefinirMotoristasAsync();
                await DefinirProdutosAsync();
            }
        }

        private async Task DefinirEmitentesAsync()
        {
            using (var db = new Emitentes())
            {
                Emitentes = await Task.Run(() => db.Registro.GerarObs());
                OnPropertyChanged(nameof(Emitentes));
            }
        }

        private async Task DefinirClientesAsync()
        {
            using (var db = new Clientes())
            {
                Clientes = await Task.Run(() => db.Registro.GerarObs());
                OnPropertyChanged(nameof(Clientes));
            }
        }

        private async Task DefinirMotoristasAsync()
        {
            using (var db = new Motoristas())
            {
                Motoristas = await Task.Run(() => db.Registro.GerarObs());
                OnPropertyChanged(nameof(Motoristas));
            }
        }

        private async Task DefinirProdutosAsync()
        {
            using (var db = new Produtos())
            {
                Produtos = await Task.Run(() => db.Registro.GerarObs());
                OnPropertyChanged(nameof(Produtos));
            }
        }

        private void AdicionarEmitente()
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarEmitente));
        }

        private void EditarEmitente(EmitenteDI emit)
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarEmitente), new GrupoViewBanco<EmitenteDI>
            {
                ItemBanco = emit,
                OperacaoRequirida = TipoOperacao.Edicao
            });
        }

        private async void RemoverEmitente(EmitenteDI emit)
        {
            using (var db = new Emitentes())
            {
                db.Remover(emit);
                db.SalvarMudancas();
                await DefinirEmitentesAsync();
            }
        }

        private void AdicionarCliente()
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarDestinatario));
        }

        private void EditarCliente(ClienteDI dest)
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarDestinatario), new GrupoViewBanco<ClienteDI>
            {
                ItemBanco = dest,
                OperacaoRequirida = TipoOperacao.Edicao
            });
        }

        private async void RemoverCliente(ClienteDI dest)
        {
            using (var db = new Clientes())
            {
                db.Remover(dest);
                db.SalvarMudancas();
                await DefinirClientesAsync();
            }
        }

        private void AdicionarMotorista()
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarMotorista));
        }

        private void EditarMotorista(Motorista mot)
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarMotorista), new GrupoViewBanco<Motorista>
            {
                ItemBanco = mot,
                OperacaoRequirida = TipoOperacao.Edicao
            });
        }

        private async void RemoverMotorista(Motorista mot)
        {
            using (var db = new Motoristas())
            {
                db.Remover(mot);
                db.SalvarMudancas();
                await DefinirMotoristasAsync();
            }
        }

        private void AdicionarProduto()
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarProduto));
        }

        private void EditarProduto(BaseProdutoOuServico prod)
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarProduto), new GrupoViewBanco<BaseProdutoOuServico>
            {
                ItemBanco = prod,
                OperacaoRequirida = TipoOperacao.Edicao
            });
        }

        private async void RemoverProduto(BaseProdutoOuServico prod)
        {
            using (var db = new Produtos())
            {
                db.Remover(prod);
                db.SalvarMudancas();
                await DefinirProdutosAsync();
            }
        }
    }
}
