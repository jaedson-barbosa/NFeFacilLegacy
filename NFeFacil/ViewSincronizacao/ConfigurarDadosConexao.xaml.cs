using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Sincronizacao
{
    public sealed partial class ConfigurarDadosConexao : ContentDialog
    {
        internal string IP { get; set; }
        internal int SenhaTemporaria { get; set; }

        public ConfigurarDadosConexao()
        {
            InitializeComponent();
        }
    }
}
