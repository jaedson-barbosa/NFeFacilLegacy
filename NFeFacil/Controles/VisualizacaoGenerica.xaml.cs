using NFeFacil.View;
using System.Collections;
using System.Linq;
using System.Reflection;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.Controles
{
    public sealed partial class VisualizacaoGenerica : UserControl
    {
        public new object Content
        {
            set => ObterPropriedades(value, 0);
        }

        public VisualizacaoGenerica()
        {
            InitializeComponent();
        }

        void ObterPropriedades(object obj, int profundidade)
        {
            var propriedades = obj.GetType().GetProperties();
            foreach (var prop in propriedades.Where(x => x.CanWrite
                && x.GetCustomAttribute<System.Xml.Serialization.XmlIgnoreAttribute>() == null))
            {
                var valor = prop.GetValue(obj);
                if (valor != null)
                {
                    var desc = prop.GetCustomAttribute<DescricaoPropriedade>();
                    if (valor.GetType().Namespace.Contains("NFeFacil"))
                    {
                        AdicionarCampo(desc?.Descricao ?? prop.Name, (EstilosTexto)profundidade);
                        ObterPropriedades(valor, profundidade + 1);
                    }
                    else if (valor is IEnumerable listaFilha && !(valor is string))
                    {
                        var tipoItem = listaFilha.GetType().GenericTypeArguments[0];
                        var itemPersonalizado = tipoItem.Namespace.Contains("NFeFacil");
                        foreach (var item in listaFilha)
                        {
                            if (itemPersonalizado)
                            {
                                AdicionarCampo(desc?.Descricao ?? tipoItem.Name, (EstilosTexto)profundidade);
                                ObterPropriedades(item, profundidade + 1);
                            }
                            else
                            {
                                AdicionarCampo(desc?.Descricao ?? tipoItem.Name,
                                    (EstilosTexto)profundidade, item.ToString());
                            }
                        }
                    }
                    else
                    {
                        var ext = prop.GetCustomAttribute<PropriedadeExtensivel>();
                        AdicionarCampo(ext?.NomeExtensão ?? desc?.Descricao ?? prop.Name,
                            EstilosTexto.BodyTextBlockStyle, (ext?.ObterValor(valor) ?? valor).ToString());
                    }
                }
            }
        }

        void AdicionarCampo(string texto, EstilosTexto estilo, string textoComplementar = null)
        {
            visualizacao.Inlines.Add(CriarRun(texto, textoComplementar != null));

            if (textoComplementar != null)
                visualizacao.Inlines.Add(CriarRun(textoComplementar));

            visualizacao.Inlines.Add(new LineBreak());

            Run CriarRun(string conteudo, bool label = false)
            {
                var retorno = new Run() { Text = label ? conteudo + ": " : conteudo };
                switch (estilo)
                {
                    case EstilosTexto.HeaderTextBlockStyle:
                        retorno.FontWeight = FontWeights.Light;
                        retorno.FontSize = 46;
                        return retorno;
                    case EstilosTexto.SubheaderTextBlockStyle:
                        retorno.FontWeight = FontWeights.Light;
                        retorno.FontSize = 34;
                        return retorno;
                    case EstilosTexto.TitleTextBlockStyle:
                        retorno.FontWeight = FontWeights.SemiLight;
                        retorno.FontSize = 24;
                        return retorno;
                    case EstilosTexto.SubtitleTextBlockStyle:
                        retorno.FontWeight = FontWeights.Normal;
                        retorno.FontSize = 20;
                        return retorno;
                    default:
                        retorno.FontWeight = label ? FontWeights.Bold : FontWeights.Normal;
                        return retorno;
                }
            }
        }

        enum EstilosTexto
        {
            HeaderTextBlockStyle,
            SubheaderTextBlockStyle,
            TitleTextBlockStyle,
            SubtitleTextBlockStyle,
            BodyTextBlockStyle
        }
    }
}
