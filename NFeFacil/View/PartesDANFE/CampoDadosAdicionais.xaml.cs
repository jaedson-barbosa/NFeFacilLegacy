using NFeFacil.DANFE.Pacotes;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PartesDANFE
{
    public sealed partial class CampoDadosAdicionais : UserControl
    {
        GridLength AlturaCampo => DimensoesPadrao.CentimeterToLength(3.2);

        public RichTextBlock CampoObservacoes => bloco;

        public DadosAdicionais ContextoComplexo
        {
            set
            {
                LimparBloco();
                for (int i = 0; i < value.Itens.Count; i++)
                {
                    var paragrafo = CriarParagrafo(value.Itens[i]);
                    bloco.Blocks.Add(paragrafo);
                }
            }
        }

        Paragraph CriarParagrafo(ItemDadosAdicionais item)
        {
            var paragrafo = new Paragraph() { FontSize = 8 };
            paragrafo.Inlines.Add(new Run() { Text = item.Titulo });
            paragrafo.Inlines.Add(new LineBreak());
            foreach (var linha in item.Linhas)
            {
                paragrafo.Inlines.Add(new Run { Text = $"  {linha}" });
                paragrafo.Inlines.Add(new LineBreak());
            }
            return paragrafo;
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
