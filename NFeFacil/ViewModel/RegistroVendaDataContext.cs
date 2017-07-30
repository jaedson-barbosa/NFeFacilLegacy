using NFeFacil.ItensBD;
using NFeFacil.Log;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewModel
{
    public sealed class RegistroVendaDataContext : INotifyPropertyChanged, IDisposable
    {
        public RegistroVenda ItemBanco { get; }
        TipoOperacao Operacao { get; }
        ILog Log = Popup.Current;
        Action<ExibicaoExtra, Guid> AtualizarCabecalho;
        public bool ManipulacaoAtivada { get; private set; }

        public ObservableCollection<ExibicaoProdutoVenda> ListaProdutos { get; private set; }
        public ObservableCollection<ClienteDI> Clientes { get; }
        public ObservableCollection<MotoristaDI> Motoristas { get; }

        public ICommand AdicionarProdutoCommand { get; }
        public ICommand RemoverProdutoCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand FinalizarCommand { get; }
        public ICommand AplicarDescontoCommand { get; }

        AplicativoContext db = new AplicativoContext();

        public DateTimeOffset DataHoraVenda
        {
            get => ItemBanco.DataHoraVenda;
            set => ItemBanco.DataHoraVenda = value.DateTime;
        }

        public RegistroVendaDataContext(Action<ExibicaoExtra, Guid> atualizarCabecalho)
        {
            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            RemoverProdutoCommand = new Comando<ExibicaoProdutoVenda>(RemoverProduto);
            EditarCommand = new Comando(Editar);
            FinalizarCommand = new Comando(Finalizar);
            AplicarDescontoCommand = new Comando(AplicarDesconto);

            AtualizarCabecalho = atualizarCabecalho;

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
            Operacao = TipoOperacao.Adicao;

            ManipulacaoAtivada = true;
        }

        internal RegistroVendaDataContext(RegistroVenda venda, Action<ExibicaoExtra, Guid> atualizarCabecalho)
        {
            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            RemoverProdutoCommand = new Comando<ExibicaoProdutoVenda>(RemoverProduto);
            EditarCommand = new Comando(Editar);
            FinalizarCommand = new Comando(Finalizar);
            AplicarDescontoCommand = new Comando(AplicarDesconto);

            AtualizarCabecalho = atualizarCabecalho;

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
            Operacao = TipoOperacao.Edicao;

            ManipulacaoAtivada = false;
        }

        private void ListaProdutos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var removido = (ExibicaoProdutoVenda)e.OldItems[0];
                ListaProdutos.Remove(removido);
                ItemBanco.Produtos.Remove(removido.Base);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        async void AdicionarProduto()
        {
            var caixa = new View.CaixasDialogo.RegistroVenda.AdicionarProduto();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var contexto = (AdicionarProdutoVendaDataContext)caixa.DataContext;
                var novoProdBanco = new ProdutoSimplesVenda
                {
                    IdBase = contexto.ProdutoSelecionado.Base.Id,
                    ValorUnitario = contexto.ProdutoSelecionado.PreçoDouble,
                    Quantidade = contexto.Quantidade,
                    Frete = contexto.Frete,
                    Seguro = contexto.Seguro,
                    DespesasExtras = contexto.DespesasExtras
                };
                novoProdBanco.CalcularTotalLíquido();
                var novoProdExib = new ExibicaoProdutoVenda
                {
                    Base = novoProdBanco,
                    Descricao = contexto.ProdutoSelecionado.Nome,
                    Quantidade = novoProdBanco.Quantidade,
                };
                ListaProdutos.Add(novoProdExib);
                ItemBanco.Produtos.Add(novoProdBanco);
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

        void Editar()
        {
            ManipulacaoAtivada = true;
            ListaProdutos.CollectionChanged += ListaProdutos_CollectionChanged;
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ManipulacaoAtivada)));
            AtualizarCabecalho(ExibicaoExtra.EscolherVendedor, default(Guid));
        }

        void Finalizar()
        {
            ItemBanco.UltimaData = DateTime.Now;
            ItemBanco.Vendedor = Propriedades.VendedorAtivo.Id;
            if (Operacao == TipoOperacao.Adicao)
            {
                db.Add(ItemBanco);
                Log.Escrever(TitulosComuns.Sucesso, "Registro de venda salvo com sucesso.");
            }
            else
            {
                db.Update(ItemBanco);
                Log.Escrever(TitulosComuns.Sucesso, "Registro de venda alterado com sucesso.");
            }
            ManipulacaoAtivada = false;
            ListaProdutos.CollectionChanged -= ListaProdutos_CollectionChanged;
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ManipulacaoAtivada)));
            db.SaveChanges();
            AtualizarCabecalho(ExibicaoExtra.ExibirVendedor, Propriedades.VendedorAtivo.Id);
        }

        async void AplicarDesconto()
        {
            var caixa = new View.CaixasDialogo.RegistroVenda.CalculoDesconto(ItemBanco.Produtos);
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
