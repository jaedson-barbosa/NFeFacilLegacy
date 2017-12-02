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
            }
            else
            {
                Produto = (ProdutoDI)e.Parameter;
            }
            DataContext = Produto;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorProduto(Produto).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        Produto.UltimaData = Propriedades.DateTimeNow;
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
    }
}
