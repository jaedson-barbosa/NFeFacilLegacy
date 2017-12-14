using NFeFacil.DANFE.Pacotes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using static NFeFacil.ExtensoesPrincipal;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.DANFE.PartesDANFE
{
    public sealed partial class CampoDadosAdicionais : UserControl
    {
        GridLength AlturaCampo => CMToLength(3.2);

        public RichTextBlock CampoObservacoes => bloco;

        public DadosAdicionais Contexto
        {
            set
            {
                for (int i = 1; i < bloco.Blocks.Count; i++) bloco.Blocks.RemoveAt(i);
                value.Itens.ForEach(x => bloco.Blocks.Add(CriarParagrafo(x)));
            }
        }

        Paragraph CriarParagrafo(ItemDadosAdicionais item)
        {
            var paragrafo = new Paragraph() { FontSize = 10 };
            paragrafo.Inlines.Add(new Run() { Text = $"    {item.Titulo}" });
            paragrafo.Inlines.Add(new LineBreak());
            foreach (var linha in item.Linhas)
            {
                paragrafo.Inlines.Add(new Run { Text = $"{linha}" });
                paragrafo.Inlines.Add(new LineBreak());
            }
            return paragrafo;
        }

        public CampoDadosAdicionais()
        {
            InitializeComponent();
        }
    }
}
