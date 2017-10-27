using NFeFacil.ItensBD;
using NFeFacil.Controles;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarDadosBase : Page, IHambuguer
    {
        public GerenciarDadosBase()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.Where(x => x.Ativo).OrderBy(x => x.Nome).GerarObs();
                Motoristas = db.Motoristas.Where(x => x.Ativo).OrderBy(x => x.Nome).GerarObs();
                Produtos = db.Produtos.Where(x => x.Ativo).OrderBy(x => x.Descricao).GerarObs();

                var vendedores = db.Vendedores.Where(x => x.Ativo).ToArray();
                var imagens = db.Imagens;
                var quantVendedores = vendedores.Length;
                var conjuntos = new ObservableCollection<ExibicaoVendedor>();
                for (int i = 0; i < quantVendedores; i++)
                {
                    var atual = vendedores[i];
                    var novoConjunto = new ExibicaoVendedor
                    {
                        Id = atual.Id,
                        Principal = atual.Nome,
                        Secundario = atual.CPF.ToString("000,000,000-00"),
                        Vendedor = atual
                    };
                    var img = imagens.Find(atual.Id);
                    if (img != null && img.Bytes != null)
                    {
                        var task = img.GetSourceAsync();
                        task.Wait();
                        novoConjunto.Imagem = task.Result;
                    }
                    conjuntos.Add(novoConjunto);
                }
                Vendedores = conjuntos.OrderBy(x => x.Principal).GerarObs();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Manage, "Dados base");
        }

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.People, "Clientes"),
            new ItemHambuguer(Symbol.People, "Motoristas"),
            new ItemHambuguer(Symbol.Shop, "Produtos"),
            new ItemHambuguer(Symbol.People, "Vendedores")
        };

        ObservableCollection<ClienteDI> Clientes { get; }
        ObservableCollection<MotoristaDI> Motoristas { get; }
        ObservableCollection<ProdutoDI> Produtos { get; }
        ObservableCollection<ExibicaoVendedor> Vendedores { get; }

        public void AtualizarMain(int index) => flipView.SelectedIndex = index;

        private void TelaMudou(object sender, SelectionChangedEventArgs e)
        {
            var index = ((FlipView)sender).SelectedIndex;
            MainPage.Current.AlterarSelectedIndexHamburguer(index);
        }

        async void AdicionarCliente()
        {
            var caixa = new EscolherTipoCliente();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                switch (caixa.TipoCliente)
                {
                    case 0:
                        MainPage.Current.Navegar<AdicionarClienteBrasileiro>();
                        break;
                    case 1:
                        MainPage.Current.Navegar<AdicionarClienteBrasileiroPFContribuinte>();
                        break;
                    case 2:
                        MainPage.Current.Navegar<AdicionarClienteBrasileiroPJ>();
                        break;
                    case 3:
                        MainPage.Current.Navegar<AdicionarClienteEstrangeiro>();
                        break;
                }
            }
        }

        private void EditarCliente(ClienteDI dest)
        {
            if (!string.IsNullOrEmpty(dest.CPF))
            {
                if (dest.IndicadorIE == 1)
                {
                    MainPage.Current.Navegar<AdicionarClienteBrasileiroPFContribuinte>(dest);
                }
                else
                {
                    MainPage.Current.Navegar<AdicionarClienteBrasileiro>(dest);
                }
            }
            else if (!string.IsNullOrEmpty(dest.CNPJ))
            {
                MainPage.Current.Navegar<AdicionarClienteBrasileiroPJ>(dest);
            }
            else
            {
                MainPage.Current.Navegar<AdicionarClienteEstrangeiro>(dest);
            }
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

        void AdicionarVendedor()
        {
            MainPage.Current.Navegar<AdicionarVendedor>();
        }

        void EditarVendedor(ExibicaoVendedor vend)
        {
            MainPage.Current.Navegar<AdicionarVendedor>(vend.Vendedor);
        }

        void InativarVendedor(ExibicaoVendedor exib)
        {
            var vend = exib.Vendedor;
            using (var db = new AplicativoContext())
            {
                vend.Ativo = false;
                db.Update(vend);
                db.SaveChanges();
                Vendedores.Remove(exib);
            }
        }

        private void AdicionarCliente(object sender, RoutedEventArgs e)
        {
            AdicionarCliente();
        }

        private void EditarCliente(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            EditarCliente((ClienteDI)contexto);
        }

        private void InativarCliente(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            InativarCliente((ClienteDI)contexto);
        }

        private void AdicionarMotorista(object sender, RoutedEventArgs e)
        {
            AdicionarMotorista();
        }

        private void EditarMotorista(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            EditarMotorista((MotoristaDI)contexto);
        }

        private void InativarMotorista(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            InativarMotorista((MotoristaDI)contexto);
        }

        private void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            AdicionarProduto();
        }

        private void EditarProduto(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            EditarProduto((ProdutoDI)contexto);
        }

        private void InativarProduto(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            InativarProduto((ProdutoDI)contexto);
        }

        private void AdicionarVendedor(object sender, RoutedEventArgs e)
        {
            AdicionarVendedor();
        }

        private void EditarVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            EditarVendedor((ExibicaoVendedor)contexto);
        }

        private void InativarVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            InativarVendedor((ExibicaoVendedor)contexto);
        }

        async void ImagemVendedor(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var vend = (ExibicaoVendedor)contexto;
            var caixa = new View.DefinirImagem(vend.Id, vend.Imagem);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var index = Vendedores.IndexOf(vend);
                vend.Imagem = caixa.Imagem;
                Vendedores[index] = vend;
            }
        }

        sealed class ExibicaoVendedor : ConjuntoBasicoExibicao
        {
            internal Vendedor Vendedor { get; set; }
        }
    }
}
