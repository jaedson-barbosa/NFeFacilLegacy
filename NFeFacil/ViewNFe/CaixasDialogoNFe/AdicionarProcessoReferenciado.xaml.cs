using NFeFacil.ModeloXML.PartesDetalhes;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoNFe
{
    public sealed partial class AdicionarProcessoReferenciado : ContentDialog
    {
        public AdicionarProcessoReferenciado()
        {
            InitializeComponent();
            Item = new ProcessoReferenciado();
        }

        public ProcessoReferenciado Item { get; set; }

        public int Origem
        {
            get => Item.IndProc == 9 ? 4 : Item.IndProc;
            set => Item.IndProc = value == 4 ? 9 : value;
        }
    }
}
