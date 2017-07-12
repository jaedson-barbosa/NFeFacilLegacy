using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewModel
{
    public sealed class RegistroVendaDataContext : INotifyPropertyChanged
    {
        public RegistroVenda ItemBanco { get; }
        TipoOperacao Operacao { get; }
        ILog Log = Popup.Current;
        public bool ManipulacaoAtivada { get; private set; } = true;

        public ObservableCollection<ProdutoSimplesVenda> ListaProdutos { get; private set; }
        public ObservableCollection<ClienteDI> Clientes { get; }
        public ObservableCollection<MotoristaDI> Motoristas { get; }

        public ICommand AdicionarProdutoCommand { get; }
        public ICommand RemoverProdutoCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand FinalizarCommand { get; }

        public RegistroVendaDataContext()
        {
            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            RemoverProdutoCommand = new Comando<ProdutoSimplesVenda>(RemoverProduto);
            EditarCommand = new Comando(Editar);
            FinalizarCommand = new Comando(Finalizar);

            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.GerarObs();
                Motoristas = db.Motoristas.GerarObs();
            }

            ItemBanco = new RegistroVenda
            {
                Emitente = Propriedades.EmitenteAtivo,
                Cliente = Clientes[0],
                Produtos = new System.Collections.Generic.List<ProdutoSimplesVenda>(),
                DataHoraVenda = DateTime.Now
            };
            Operacao = TipoOperacao.Adicao;
        }

        internal RegistroVendaDataContext(RegistroVenda venda, TipoOperacao operacao)
        {
            AdicionarProdutoCommand = new Comando(AdicionarProduto);
            RemoverProdutoCommand = new Comando<ProdutoSimplesVenda>(RemoverProduto);
            EditarCommand = new Comando(Editar);
            FinalizarCommand = new Comando(Finalizar);

            using (var db = new AplicativoContext())
            {
                Clientes = db.Clientes.GerarObs();
                Motoristas = db.Motoristas.GerarObs();
            }

            ItemBanco = venda;
            Operacao = operacao;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        async void AdicionarProduto()
        {
            var caixa = new View.CaixasDialogo.RegistroVenda.AdicionarProduto();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var contexto = (AdicionarProdutoVendaDataContext)caixa.DataContext;
                var novoProd = new ProdutoSimplesVenda
                {
                    ProdutoBase = contexto.ProdutoSelecionado.Base,
                    Quantidade = contexto.Quantidade,
                    Frete = contexto.Frete,
                    Seguro = contexto.Seguro,
                    DespesasExtras = contexto.DespesasExtras
                };
                novoProd.CalcularTotalLíquido();
                ListaProdutos.Add(novoProd);
                ItemBanco.Produtos.Add(novoProd);
            }
        }

        void RemoverProduto(ProdutoSimplesVenda prod)
        {
            ListaProdutos.Remove(prod);
            ItemBanco.Produtos.Remove(prod);
        }

        void Editar()
        {
            ManipulacaoAtivada = true;
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
                db.SaveChanges();
            }
        }
    }
}
