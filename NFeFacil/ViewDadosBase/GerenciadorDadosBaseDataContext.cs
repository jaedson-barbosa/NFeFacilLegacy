using NFeFacil.ItensBD;
using NFeFacil.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NFeFacil.ViewDadosBase
{
    public sealed class GerenciadorDadosBaseDataContext
    {
        public ObservableCollection<ClienteDI> Clientes { get; private set; }
        public ObservableCollection<MotoristaDI> Motoristas { get; private set; }
        public ObservableCollection<ProdutoDI> Produtos { get; private set; }
        public ObservableCollection<Vendedor> Vendedores { get; private set; }

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
            AdicionarClienteCommand = new Comando(AdicionarCliente);
            EditarClienteCommand = new Comando<ClienteDI>(EditarCliente);
            RemoverClienteCommand = new Comando<ClienteDI>(RemoverCliente);

            AdicionarMotoristaCommand = new Comando(AdicionarMotorista);
            EditarMotoristaCommand = new Comando<MotoristaDI>(EditarMotorista);
            RemoverMotoristaCommand = new Comando<MotoristaDI>(RemoverMotorista);

            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            EditarProdutoCommand = new Comando<ProdutoDI>(EditarProduto);
            RemoverProdutoCommand = new Comando<ProdutoDI>(RemoverProduto);

            AdicionarVendedorCommand = new Comando(AdicionarVendedor);
            EditarVendedorCommand = new Comando<Vendedor>(EditarVendedor);
            RemoverVendedorCommand = new Comando<Vendedor>(RemoverVendedor);

            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.GerarObs();
                Motoristas = db.Motoristas.GerarObs();
                Produtos = db.Produtos.GerarObs();
                Vendedores = db.Vendedores.GerarObs();
            }
        }

        private void AdicionarCliente()
        {
            MainPage.Current.Navegar<AdicionarDestinatario>();
        }

        private void EditarCliente(ClienteDI dest)
        {
            MainPage.Current.Navegar<AdicionarDestinatario>(new GrupoViewBanco<ClienteDI>
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
                Clientes.Remove(dest);
            }
        }

        private void AdicionarMotorista()
        {
            MainPage.Current.Navegar<AdicionarMotorista>();
        }

        private void EditarMotorista(MotoristaDI mot)
        {
            MainPage.Current.Navegar<AdicionarMotorista>(new GrupoViewBanco<MotoristaDI>
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
                Motoristas.Remove(mot);
            }
        }

        private void AdicionarProduto()
        {
            MainPage.Current.Navegar<AdicionarProduto>();
        }

        private void EditarProduto(ProdutoDI prod)
        {
            MainPage.Current.Navegar<AdicionarProduto>(new GrupoViewBanco<ProdutoDI>
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
                Produtos.Remove(prod);
            }
        }

        public void AdicionarVendedor()
        {
            MainPage.Current.Navegar<AdicionarVendedor>();
        }

        public void EditarVendedor(Vendedor vend)
        {
            MainPage.Current.Navegar<AdicionarVendedor>(new GrupoViewBanco<Vendedor>
            {
                ItemBanco = vend,
                OperacaoRequirida = TipoOperacao.Edicao
            });
        }

        public void RemoverVendedor(Vendedor vend)
        {
            using (var db = new AplicativoContext())
            {
                db.Remove(vend);
                db.SaveChanges();
                Vendedores.Remove(vend);
            }
        }
    }
}
