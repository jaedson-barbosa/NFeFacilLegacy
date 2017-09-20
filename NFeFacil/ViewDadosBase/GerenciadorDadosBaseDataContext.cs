using NFeFacil.ItensBD;
using NFeFacil.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ICommand InativarClienteCommand { get; }

        public ICommand AdicionarMotoristaCommand { get; }
        public ICommand EditarMotoristaCommand { get; }
        public ICommand InativarMotoristaCommand { get; }

        public ICommand AdicionarProdutoCommand { get; }
        public ICommand EditarProdutoCommand { get; }
        public ICommand InativarProdutoCommand { get; }

        public ICommand AdicionarVendedorCommand { get; }
        public ICommand EditarVendedorCommand { get; }
        public ICommand InativarVendedorCommand { get; }

        public GerenciadorDadosBaseDataContext()
        {
            AdicionarClienteCommand = new Comando(AdicionarCliente);
            EditarClienteCommand = new Comando<ClienteDI>(EditarCliente);
            InativarClienteCommand = new Comando<ClienteDI>(InativarCliente);

            AdicionarMotoristaCommand = new Comando(AdicionarMotorista);
            EditarMotoristaCommand = new Comando<MotoristaDI>(EditarMotorista);
            InativarMotoristaCommand = new Comando<MotoristaDI>(InativarMotorista);

            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            EditarProdutoCommand = new Comando<ProdutoDI>(EditarProduto);
            InativarProdutoCommand = new Comando<ProdutoDI>(InativarProduto);

            AdicionarVendedorCommand = new Comando(AdicionarVendedor);
            EditarVendedorCommand = new Comando<Vendedor>(EditarVendedor);
            InativarVendedorCommand = new Comando<Vendedor>(InativarVendedor);

            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.Where(x => x.Ativo).GerarObs();
                Motoristas = db.Motoristas.Where(x => x.Ativo).GerarObs();
                Produtos = db.Produtos.Where(x => x.Ativo).GerarObs();
                Vendedores = db.Vendedores.Where(x => x.Ativo).GerarObs();
            }
        }

        private void AdicionarCliente()
        {
            MainPage.Current.Navegar<AdicionarDestinatario>();
        }

        private void EditarCliente(ClienteDI dest)
        {
            MainPage.Current.Navegar<AdicionarDestinatario>(dest);
        }

        private void InativarCliente(ClienteDI dest)
        {
            using (var db = new AplicativoContext())
            {
                dest.Ativo = false;
                db.Update(dest);
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
            MainPage.Current.Navegar<AdicionarMotorista>(mot);
        }

        private void InativarMotorista(MotoristaDI mot)
        {
            using (var db = new AplicativoContext())
            {
                mot.Ativo = false;
                db.Update(mot);
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
            MainPage.Current.Navegar<AdicionarProduto>(prod);
        }

        private void InativarProduto(ProdutoDI prod)
        {
            using (var db = new AplicativoContext())
            {
                prod.Ativo = false;
                db.Update(prod);
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
            MainPage.Current.Navegar<AdicionarVendedor>(vend);
        }

        public void InativarVendedor(Vendedor vend)
        {
            using (var db = new AplicativoContext())
            {
                vend.Ativo = false;
                db.Update(vend);
                db.SaveChanges();
                Vendedores.Remove(vend);
            }
        }
    }
}
