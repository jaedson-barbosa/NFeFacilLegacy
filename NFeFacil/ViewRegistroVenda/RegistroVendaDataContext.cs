using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewRegistroVenda
{
    public sealed class RegistroVendaDataContext : INotifyPropertyChanged, IDisposable
    {
        public RegistroVenda ItemBanco { get; }
        ILog Log = Popup.Current;

        public ObservableCollection<ExibicaoProdutoVenda> ListaProdutos { get; private set; }
        public ObservableCollection<ClienteDI> Clientes { get; }
        public ObservableCollection<MotoristaDI> Motoristas { get; }

        public ICommand AdicionarProdutoCommand { get; }
        public ICommand RemoverProdutoCommand { get; }
        public ICommand FinalizarCommand { get; }
        public ICommand AplicarDescontoCommand { get; }

        public string ValorTotal => ItemBanco.Produtos.Sum(x => x.TotalLíquido).ToString("C");

        AplicativoContext db = new AplicativoContext();

        public DateTimeOffset DataHoraVenda
        {
            get => ItemBanco.DataHoraVenda;
            set => ItemBanco.DataHoraVenda = value.DateTime;
        }

        public RegistroVendaDataContext()
        {
            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            RemoverProdutoCommand = new Comando<ExibicaoProdutoVenda>(RemoverProduto);
            FinalizarCommand = new Comando(Finalizar);
            AplicarDescontoCommand = new Comando(AplicarDesconto);

            Clientes = db.Clientes.GerarObs();
            Motoristas = db.Motoristas.GerarObs();

            ListaProdutos = new ObservableCollection<ExibicaoProdutoVenda>();
            ItemBanco = new RegistroVenda
            {
                Emitente = Propriedades.EmitenteAtivo.Id,
                Cliente = Clientes[0].Id,
                Produtos = new System.Collections.Generic.List<ProdutoSimplesVenda>(),
                DataHoraVenda = DateTime.Now
            };
        }

        internal RegistroVendaDataContext(RegistroVenda venda)
        {
            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            RemoverProdutoCommand = new Comando<ExibicaoProdutoVenda>(RemoverProduto);
            FinalizarCommand = new Comando(Finalizar);
            AplicarDescontoCommand = new Comando(AplicarDesconto);

            db.AttachRange(venda.Produtos);
            Clientes = db.Clientes.GerarObs();
            Motoristas = db.Motoristas.GerarObs();
            ListaProdutos = (from prod in venda.Produtos
                             select new ExibicaoProdutoVenda
                             {
                                 Base = prod,
                                 Descricao = db.Produtos.Find(prod.IdBase).Descricao,
                                 Quantidade = prod.Quantidade,
                             }).GerarObs();

            ItemBanco = venda;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        async void AdicionarProduto()
        {
            var caixa = new AdicionarProduto();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoProdBanco = new ProdutoSimplesVenda
                {
                    IdBase = caixa.ProdutoSelecionado.Base.Id,
                    ValorUnitario = caixa.ProdutoSelecionado.PreçoDouble,
                    Quantidade = caixa.Quantidade,
                    Frete = caixa.Frete,
                    Seguro = caixa.Seguro,
                    DespesasExtras = caixa.DespesasExtras
                };
                novoProdBanco.CalcularTotalLíquido();
                var novoProdExib = new ExibicaoProdutoVenda
                {
                    Base = novoProdBanco,
                    Descricao = caixa.ProdutoSelecionado.Nome,
                    Quantidade = novoProdBanco.Quantidade,
                };
                ListaProdutos.Add(novoProdExib);
                ItemBanco.Produtos.Add(novoProdBanco);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ValorTotal)));
            }
        }

        void RemoverProduto(ExibicaoProdutoVenda prod)
        {
            if (prod.Base.Id != default(Guid))
            {
                db.Remove(prod.Base);
            }
            ListaProdutos.Remove(prod);
            ItemBanco.Produtos.Remove(prod.Base);
        }

        void Finalizar()
        {
            ItemBanco.UltimaData = DateTime.Now;
            ItemBanco.Vendedor = Propriedades.VendedorAtivo?.Id ?? Guid.Empty;
            if (ItemBanco.Id == Guid.Empty)
            {
                db.Add(ItemBanco);
                ItemBanco.Produtos.ForEach(x => x.RegistrarAlteracaoEstoque(db));
                Log.Escrever(TitulosComuns.Sucesso, "Registro de venda salvo com sucesso.");
            }
            else
            {
                var antigo = db.Vendas.Find(ItemBanco.Id);
                antigo.Produtos.ForEach(x => x.DesregistrarAlteracaoEstoque(db));
                ItemBanco.Produtos.ForEach(x => x.RegistrarAlteracaoEstoque(db));
                db.Update(ItemBanco);
                Log.Escrever(TitulosComuns.Sucesso, "Registro de venda alterado com sucesso.");
            }
            db.SaveChanges();
        }

        async void AplicarDesconto()
        {
            var caixa = new CalculoDesconto(ItemBanco.Produtos);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var prods = caixa.Produtos;
                for (int i = 0; i < prods.Count; i++)
                {
                    var atual = prods[i];
                    atual.CalcularTotalLíquido();
                    var antigo = ListaProdutos[i];
                    antigo.Base = atual;
                    ListaProdutos[i] = antigo;
                    ItemBanco.Produtos[i] = antigo.Base;
                }
                ItemBanco.DescontoTotal = prods.Sum(x => x.Desconto);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ItemBanco)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListaProdutos)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ValorTotal)));
            }
        }

        public void Dispose()
        {

            db.Dispose();
        }

        public struct ExibicaoProdutoVenda
        {
            public ProdutoSimplesVenda Base { get; set; }
            public string Descricao { get; set; }
            public double Quantidade { get; set; }
            public string TotalLíquido => Base.TotalLíquido.ToString("C");
        }
    }
}
