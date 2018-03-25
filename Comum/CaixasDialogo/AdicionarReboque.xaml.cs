using BaseGeral.ModeloXML.PartesDetalhes.PartesTransporte;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Comum.CaixasDialogo
{
    public sealed partial class AdicionarReboque : ContentDialog
    {
        public Reboque Contexto { get; } = new Reboque();

        public AdicionarReboque()
        {
            InitializeComponent();
        }
    }
}
