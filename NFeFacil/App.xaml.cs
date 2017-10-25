using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using NFeFacil.Sincronizacao;
using System;
using System.Threading.Tasks;
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
            var task = AnaliseBanco();
            task.Start();
            task.Wait();
            IBGE.Estados.Buscar();
            IBGE.Municipios.Buscar();
            if (ConfiguracoesSincronizacao.InícioAutomático)
            {
                GerenciadorServidor.Current.IniciarServer().ConfigureAwait(false);
            }
        }

        async Task AnaliseBanco()
        {
            using (var db = new AplicativoContext())
            {
                db.Database.Migrate();
                await db.Clientes.ForEachAsync(x => AnalisarItem(x));
                await db.Emitentes.ForEachAsync(x => AnalisarItem(x));
                await db.Motoristas.ForEachAsync(x => AnalisarItem(x));
                await db.Vendedores.ForEachAsync(x => AnalisarItem(x));
                await db.Produtos.ForEachAsync(x => AnalisarItem(x));
                await db.Estoque.ForEachAsync(x => AnalisarItem(x));
                await db.Vendas.ForEachAsync(x => AnalisarItem(x));
                await db.Imagens.ForEachAsync(x => AnalisarItem(x));
                db.SaveChanges();
            }

            void AnalisarItem(IUltimaData item)
            {
                if (item.UltimaData == DateTime.MinValue)
                {
                    item.UltimaData = DateTime.Now;
                }
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
    }
}
