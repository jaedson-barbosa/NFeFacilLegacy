using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewModel
{
    public sealed class RegistroVendaDataContext : INotifyPropertyChanged
    {
        public RegistroVenda ItemBanco { get; }
        TipoOperacao Operacao { get; }
        ILog Log = Popup.Current;
        public bool ManipulacaoAtivada { get; private set; }

        public ObservableCollection<ExibicaoProdutoVenda> ListaProdutos { get; private set; }
        public ObservableCollection<ClienteDI> Clientes { get; }
        public ObservableCollection<MotoristaDI> Motoristas { get; }

        public ICommand AdicionarProdutoCommand { get; }
        public ICommand RemoverProdutoCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand FinalizarCommand { get; }

        public DateTimeOffset DataHoraVenda
        {
            get => ItemBanco.DataHoraVenda;
            set => ItemBanco.DataHoraVenda = value.DateTime;
        }

        public RegistroVendaDataContext()
        {
            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            RemoverProdutoCommand = new Comando<ExibicaoProdutoVenda>(RemoverProduto);
            EditarCommand = new Comando(Editar);
            FinalizarCommand = new Comando(Finalizar);

            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.GerarObs();
                Motoristas = db.Motoristas.GerarObs();
            }

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

        internal RegistroVendaDataContext(RegistroVenda venda)
        {
            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            RemoverProdutoCommand = new Comando<ExibicaoProdutoVenda>(RemoverProduto);
            EditarCommand = new Comando(Editar);
            FinalizarCommand = new Comando(Finalizar);

            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.GerarObs();
                Motoristas = db.Motoristas.GerarObs();
                ListaProdutos = new ObservableCollection<ExibicaoProdutoVenda>(from prod in venda.Produtos
                                                                               select new ExibicaoProdutoVenda
                                                                               {
                                                                                   Base = prod,
                                                                                   Descricao = db.Produtos.Find(prod.IdBase).Descricao,
                                                                                   Quantidade = prod.Quantidade,
                                                                                   TotalLíquido = prod.TotalLíquido.ToString("C")
                                                                               });
            }

            ItemBanco = venda;
            Operacao = TipoOperacao.Edicao;

            ManipulacaoAtivada = false;
        }

        private void ListaProdutos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
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
                    TotalLíquido = novoProdBanco.TotalLíquido.ToString("C")
                };
                ListaProdutos.Add(novoProdExib);
                ItemBanco.Produtos.Add(novoProdBanco);
            }
        }

        void RemoverProduto(ExibicaoProdutoVenda prod)
        {
            ListaProdutos.Remove(prod);
            ItemBanco.Produtos.Remove(prod.Base);
        }

        void Editar()
        {
            ManipulacaoAtivada = true;
            ListaProdutos.CollectionChanged += ListaProdutos_CollectionChanged;
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ManipulacaoAtivada)));
        }

        void Finalizar()
        {
            using (var db = new AplicativoContext())
            {
                ItemBanco.UltimaData = DateTime.Now;
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
            }
        }

        public struct ExibicaoProdutoVenda
        {
            public ProdutoSimplesVenda Base { get; set; }
            public string Descricao { get; set; }
            public double Quantidade { get; set; }
            public string TotalLíquido { get; set; }
        }
    }
}
