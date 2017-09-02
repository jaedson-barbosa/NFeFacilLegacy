using NFeFacil.Log;
using NFeFacil.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using NFeFacil.ItensBD;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarProduto : Page
    {
        private ProdutoDI Produto;
        private ILog Log = Popup.Current;

        public AdicionarProduto()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                Produto = new ProdutoDI();
                MainPage.Current.SeAtualizar(Symbol.Add, "Produto");
                chkControleEstoque.IsEnabled = false;
            }
            else
            {
                Produto = (ProdutoDI)e.Parameter;
                MainPage.Current.SeAtualizar(Symbol.Edit, "Produto");
                using (var db = new AplicativoContext())
                    chkControleEstoque.IsChecked = db.Estoque.Find(Produto.Id) != null;
            }
            DataContext = Produto;
            chkControleEstoque.Checked += ControleEstoque_Checked;
            chkControleEstoque.Unchecked += ControleEstoque_Unchecked;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chkControleEstoque.Checked -= ControleEstoque_Checked;
                chkControleEstoque.Unchecked -= ControleEstoque_Unchecked;
                if (new ValidadorProduto(Produto).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        Produto.UltimaData = DateTime.Now;
                        if (Produto.Id == Guid.Empty)
                        {
                            db.Add(Produto);
                            Log.Escrever(TitulosComuns.Sucesso, "Produto salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(Produto);
                            Log.Escrever(TitulosComuns.Sucesso, "Produto alterado com sucesso.");
                        }
                        db.SaveChanges();
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }

        private void ControleEstoque_Checked(object sender, RoutedEventArgs e)
        {
            using (var db = new AplicativoContext())
            {
                db.Estoque.Add(new Estoque() { Id = Produto.Id });
                db.SaveChanges();
            }
        }

        private void ControleEstoque_Unchecked(object sender, RoutedEventArgs e)
        {
            using (var db = new AplicativoContext())
            {
                var item = db.Estoque.Find(Produto.Id);
                if (item != null) db.Estoque.Remove(item);
                db.SaveChanges();
            }
        }
    }
}
