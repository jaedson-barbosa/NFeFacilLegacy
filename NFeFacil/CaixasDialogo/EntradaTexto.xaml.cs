using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.CaixasDialogo
{
    public sealed partial class EntradaTexto : ContentDialog
    {
        string Titulo { get; }
        string Cabecalho { get; }
        public string Conteudo { get; private set; }

        public EntradaTexto(string titulo, string cabecalho)
        {
            Titulo = titulo;
            Cabecalho = cabecalho;
            this.InitializeComponent();
        }
    }
}
