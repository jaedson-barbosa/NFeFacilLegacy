using BibliotecaCentral.Sincronizacao;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
            Suspending += OnSuspending;
            BibliotecaCentral.InicioGeral.IniciarBancoDados();
            BibliotecaCentral.InicioGeral.IniciarIBGE();
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

        /// <summary>
        /// Chamado quando a execução do aplicativo está sendo suspensa.  O estado do aplicativo é salvo
        /// sem saber se o aplicativo será encerrado ou retomado com o conteúdo
        /// da memória ainda intacto.
        /// </summary>
        /// <param name="sender">A fonte da solicitação de suspensão.</param>
        /// <param name="e">Detalhes sobre a solicitação de suspensão.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Salvar o estado do aplicativo e parar qualquer atividade em segundo plano
            deferral.Complete();
        }
    }
}
