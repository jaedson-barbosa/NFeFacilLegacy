using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Repositorio;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<MotoristaDI> Motoristas { get; private set; }
        public ObservableCollection<ProdutoDI> Produtos { get; private set; }
        public ObservableCollection<Vendedor> Vendedores { get; private set; }

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

        public ICommand AdicionarVendedorCommand { get; }
        public ICommand EditarVendedorCommand { get; }
        public ICommand RemoverVendedorCommand { get; }

        public GerenciadorDadosBaseDataContext()
        {
            AdicionarEmitenteCommand = new Comando(AdicionarEmitente);
            EditarEmitenteCommand = new Comando<EmitenteDI>(EditarEmitente);
            RemoverEmitenteCommand = new Comando<EmitenteDI>(RemoverEmitente);

            AdicionarClienteCommand = new Comando(AdicionarCliente);
            EditarClienteCommand = new Comando<ClienteDI>(EditarCliente);
            RemoverClienteCommand = new Comando<ClienteDI>(RemoverCliente);

            AdicionarMotoristaCommand = new Comando(AdicionarMotorista);
            EditarMotoristaCommand = new Comando<MotoristaDI>(EditarMotorista);
            RemoverMotoristaCommand = new Comando<MotoristaDI>(RemoverMotorista);

            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            EditarProdutoCommand = new Comando<ProdutoDI>(EditarProduto);
            RemoverProdutoCommand = new Comando<ProdutoDI>(RemoverProduto);

            using (var db = new AplicativoContext())
            {
                Emitentes = db.Emitentes.GerarObs();
                Clientes = db.Clientes.GerarObs();
                Motoristas = db.Motoristas.GerarObs();
                Produtos = db.Produtos.GerarObs();
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

        private void RemoverEmitente(EmitenteDI emit)
        {
            using (var db = new AplicativoContext())
            {
                db.Remove(emit);
                db.SaveChanges();
                Emitentes = db.Emitentes.GerarObs();
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

        private void RemoverCliente(ClienteDI dest)
        {
            using (var db = new AplicativoContext())
            {
                db.Remove(dest);
                db.SaveChanges();
                Clientes = db.Clientes.GerarObs();
            }
        }

        private void AdicionarMotorista()
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarMotorista));
        }

        private void EditarMotorista(MotoristaDI mot)
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarMotorista), new GrupoViewBanco<MotoristaDI>
            {
                ItemBanco = mot,
                OperacaoRequirida = TipoOperacao.Edicao
            });
        }

        private void RemoverMotorista(MotoristaDI mot)
        {
            using (var db = new AplicativoContext())
            {
                db.Remove(mot);
                db.SaveChanges();
                Motoristas = db.Motoristas.GerarObs();
            }
        }

        private void AdicionarProduto()
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarProduto));
        }

        private void EditarProduto(ProdutoDI prod)
        {
            MainPage.Current.AbrirFunçao(typeof(View.AdicionarProduto), new GrupoViewBanco<ProdutoDI>
            {
                ItemBanco = prod,
                OperacaoRequirida = TipoOperacao.Edicao
            });
        }

        private void RemoverProduto(ProdutoDI prod)
        {
            using (var db = new AplicativoContext())
            {
                db.Remove(prod);
                db.SaveChanges();
                Produtos = db.Produtos.GerarObs();
            }
        }
    }
}
