using NFeFacil.DANFE.Pacotes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PartesDANFE
{
    public sealed partial class CampoDadosAdicionais : UserControl
    {
        GridLength AlturaCampo => DimensoesPadrao.CentimeterToLength(3.2);

        public DadosAdicionais ContextoComplexo
        {
            set
            {
                LimparBloco();
                if (value.Duplicatas != null && value.Duplicatas.Count > 0)
                {
                    var paragrafo = new Paragraph();
                    paragrafo.Inlines.Add(new Run() { Text = "  DE INTERESSE DO CONTRIBUINTE:" });
                    paragrafo.Inlines.Add(new LineBreak());
                    for (int i = 0; i < value.Duplicatas.Count; i++)
                    {
                        var dup = value.Duplicatas[i];
                        paragrafo.Inlines.Add(new Run { Text = $"Duplicata - Num.: {dup.NDup}, Vec.: {dup.DVenc}, Valor: {dup.DDup.ToString("N2")}" });
                        paragrafo.Inlines.Add(new LineBreak());
                    }
                    bloco.Blocks.Add(paragrafo);
                }
                if (value.Dados != null)
                {
                    var paragrafo = new Paragraph();
                    paragrafo.Inlines.Add(new Run() { Text = "  DE INTERESSE DO CONTRIBUINTE:" });
                    paragrafo.Inlines.Add(new LineBreak());
                    paragrafo.Inlines.Add(new Run() { Text = value.Dados });
                    bloco.Blocks.Add(paragrafo);
                }
                if (value.Fisco != null)
                {
                    var paragrafo = new Paragraph();
                    paragrafo.Inlines.Add(new Run() { Text = "  DE INTERESSE DO FISCO:" });
                    paragrafo.Inlines.Add(new LineBreak());
                    paragrafo.Inlines.Add(new Run() { Text = value.Fisco });
                    bloco.Blocks.Add(paragrafo);
                }
            }
        }

        public CampoDadosAdicionais()
        {
            this.InitializeComponent();
        }

        private void LimparBloco()
        {
            if (bloco.Blocks.Count > 1)
            {
                for (int i = 1; i < bloco.Blocks.Count; i++)
                {
                    bloco.Blocks.RemoveAt(i);
                }
            }
        }
    }
}
