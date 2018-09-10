using System.Windows;

namespace ServidorCertificacao
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        Servidor servidor;

        public MainWindow()
        {
            InitializeComponent();
            servidor = new Servidor();
            servidor.OnError += Servidor_OnError;
            servidor.OnRequest += Servidor_OnRequest;
            servidor.Start();
        }

        private void Servidor_OnRequest(object sender, string e)
        {
            txtTitulo.Text = "Requisição:";
            txtCorpo.Text = e;
        }

        private void Servidor_OnError(object sender, string e)
        {
            txtTitulo.Text = "Erro:";
            txtCorpo.Text = e;
        }
    }
}
