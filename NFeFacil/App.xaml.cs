using NFeFacil.Sincronizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Services.Store;
using Windows.System;
using Windows.UI.Xaml;

namespace NFeFacil
{
    /// <summary>
    ///Fornece o comportamento específico do aplicativo para complementar a classe Application padrão.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Inicializa o objeto singleton do aplicativo.  Esta é a primeira linha de código criado
        /// executado e, como tal, é o equivalente lógico de main() ou WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            IBGE.Estados.Buscar();
            IBGE.Municipios.Buscar();
            if (ConfiguracoesSincronizacao.InícioAutomático)
            {
                GerenciadorServidor.Current.IniciarServer().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Chamado quando o aplicativo é iniciado normalmente pelo usuário final.  Outros pontos de entrada
        /// serão usados, por exemplo, quando o aplicativo for iniciado para abrir um arquivo específico.
        /// </summary>
        /// <param name="e">Detalhes sobre a solicitação e o processo de inicialização.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ObterProdutos(e.User);
            var rootFrame = Window.Current.Content as MainPage;
            if (rootFrame == null)
            {
                Window.Current.Content = rootFrame = new MainPage();
            }

            if (e.PrelaunchActivated == false)
            {
                Window.Current.Activate();
            }
        }

        async void ObterProdutos(User e)
        {
            StoreContext storeContext = StoreContext.GetDefault();
            //var resultadoAquisicao = await storeContext.RequestPurchaseAsync("9P70MWLRCS54");
            string[] productKinds = new string[] { "Consumable", "Durable", "UnmanagedConsumable" };
            var filterList = new List<string>(productKinds);
            StoreProductQueryResult addOns = await storeContext.GetAssociatedStoreProductsAsync(filterList);
            var produtos = addOns.Products.ToDictionary(x => x.Key, y => y);
            var quant = addOns.Products.Count;
            Log.Popup.Current.Escrever(Log.TitulosComuns.Log, $"Quantidade produtos: {quant}");
        }
    }
}
