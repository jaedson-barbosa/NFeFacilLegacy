using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.Validacao;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarVendedor : Page
    {
        private Vendedor vendedor;
        private ILog Log = Popup.Current;

        public AdicionarVendedor()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                vendedor = new Vendedor();
                MainPage.Current.SeAtualizar(Symbol.Add, "Vendedor");
            }
            else
            {
                vendedor = (Vendedor)e.Parameter;
                MainPage.Current.SeAtualizar(Symbol.Edit, "Vendedor");
            }
            DataContext = vendedor;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorVendedor(vendedor).Validar(Log))
                {
                    using (var db = new AplicativoContext())
                    {
                        vendedor.UltimaData = Propriedades.DateTimeNow;
                        if (vendedor.Id == Guid.Empty)
                        {
                            db.Add(vendedor);
                            Log.Escrever(TitulosComuns.Sucesso, "Vendedor salvo com sucesso.");
                        }
                        else
                        {
                            db.Update(vendedor);
                            Log.Escrever(TitulosComuns.Sucesso, "Vendedor alterado com sucesso.");
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
