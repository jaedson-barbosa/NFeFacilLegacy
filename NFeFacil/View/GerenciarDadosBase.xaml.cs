using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarDadosBase : Page
    {
        public GerenciarDadosBase()
        {
            InitializeComponent();
            BarraTitulo.Current.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Manage, "Gerenciar dados base");
        }
    }
}
