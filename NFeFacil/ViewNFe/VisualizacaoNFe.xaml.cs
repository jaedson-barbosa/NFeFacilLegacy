using NFeFacil.ModeloXML.PartesProcesso;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class VisualizacaoNFe : Page
    {
        NFe NotaFiscal { get; set; }

        public VisualizacaoNFe()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NotaFiscal = (NFe)e.Parameter;
            var propriedades = ObterPropriedades(NotaFiscal.Informações);
            var linear = PropriedadeHierarquicaToLinear(propriedades, 0);
            linear.ForEach(x => AdicionarCampo(x.Texto, (EstilosTexto)x.Profundidade, x.Complementar));
        }

        List<PropriedadeHierárquica> ObterPropriedades(object obj)
        {
            var retorno = new List<PropriedadeHierárquica>();
            var tipo = obj.GetType();
            foreach (var prop in tipo.GetProperties())
            {
                var valor = prop.GetValue(obj);
                if (valor != null)
                {
                    var tipoFilho = valor.GetType();
                    if (tipoFilho.Namespace.Contains("NFeFacil") && !(valor is IEnumerable))
                    {
                        retorno.Add(new PropriedadeHierárquica
                        {
                            Nome = prop.Name,
                            Valor = ObterPropriedades(valor)
                        });
                    }
                    else if (valor is IEnumerable teste && !(valor is string))
                    {
                        List<PropriedadeHierárquica> propriedadesFilhas = new List<PropriedadeHierárquica>();
                        foreach (var item in teste)
                        {
                            var tipoItem = item.GetType();
                            object valorItem;
                            if (tipoItem.Namespace.Contains("NFeFacil"))
                            {
                                valorItem = ObterPropriedades(item);
                            }
                            else
                            {
                                valorItem = item;
                            }
                            propriedadesFilhas.Add(new PropriedadeHierárquica
                            {
                                Nome = tipoItem.Name,
                                Valor = valorItem
                            });
                        }
                        retorno.AddRange(propriedadesFilhas);
                    }
                    else
                    {
                        var desc = prop.GetCustomAttribute<DescricaoPropriedade>();
                        retorno.Add(new PropriedadeHierárquica
                        {
                            Nome = desc != null ? desc.Descricao : prop.Name,
                            Valor = valor
                        });
                    }
                }
            }
            return retorno;
        }

        List<PropriedadeLinear> PropriedadeHierarquicaToLinear(List<PropriedadeHierárquica> hierarquia, int profundidade)
        {
            var retorno = new List<PropriedadeLinear>();
            for (int i = 0; i < hierarquia.Count; i++)
            {
                var atual = hierarquia[i];
                if (atual.Valor is List<PropriedadeHierárquica> subhierarquia)
                {
                    retorno.Add(new PropriedadeLinear
                    {
                        Profundidade = profundidade,
                        Texto = atual.Nome
                    });
                    retorno.AddRange(PropriedadeHierarquicaToLinear(subhierarquia, profundidade + 1));
                }
                else
                {
                    retorno.Add(new PropriedadeLinear
                    {
                        Profundidade = (int)EstilosTexto.BodyTextBlockStyle,
                        Texto = atual.Nome,
                        Complementar = atual.Valor.ToString()
                    });
                }
            }
            return retorno;
        }

        void AdicionarCampo(string texto, EstilosTexto estilo, string textoComplementar)
        {
            texto = ProcessarTitulo(texto);
            var linha = new Run()
            {
                Text = textoComplementar == null ? texto : $"{texto}: "
            };
            switch (estilo)
            {
                case EstilosTexto.HeaderTextBlockStyle:
                    linha.FontWeight = FontWeights.Light;
                    linha.FontSize = 46;
                    break;
                case EstilosTexto.SubheaderTextBlockStyle:
                    linha.FontWeight = FontWeights.Light;
                    linha.FontSize = 34;
                    break;
                case EstilosTexto.TitleTextBlockStyle:
                    linha.FontWeight = FontWeights.SemiLight;
                    linha.FontSize = 24;
                    break;
                case EstilosTexto.SubtitleTextBlockStyle:
                    linha.FontWeight = FontWeights.Normal;
                    linha.FontSize = 20;
                    break;
                case EstilosTexto.BodyTextBlockStyle:
                    break;
                default:
                    break;
            }

            if (textoComplementar != null)
            {
                linha.FontWeight = FontWeights.Bold;
            }

            visualizacao.Inlines.Add(linha);

            if (textoComplementar != null)
            {
                linha = new Run()
                {
                    Text = textoComplementar
                };
                switch (estilo)
                {
                    case EstilosTexto.HeaderTextBlockStyle:
                        linha.FontWeight = FontWeights.Light;
                        linha.FontSize = 46;
                        break;
                    case EstilosTexto.SubheaderTextBlockStyle:
                        linha.FontWeight = FontWeights.Light;
                        linha.FontSize = 34;
                        break;
                    case EstilosTexto.TitleTextBlockStyle:
                        linha.FontWeight = FontWeights.SemiLight;
                        linha.FontSize = 24;
                        break;
                    case EstilosTexto.SubtitleTextBlockStyle:
                        linha.FontWeight = FontWeights.Normal;
                        linha.FontSize = 20;
                        break;
                    case EstilosTexto.BodyTextBlockStyle:
                        break;
                    default:
                        break;
                }
                visualizacao.Inlines.Add(linha);
            }

            visualizacao.Inlines.Add(new LineBreak());
        }

        string ProcessarTitulo(string titulo)
        {
            StringBuilder construtor = new StringBuilder();
            construtor.Append(char.ToUpper(titulo[0]));
            for (int i = 1; i < titulo.Length; i++)
            {
                if (char.IsUpper(titulo[i]) && char.IsLower(titulo[i - 1]))
                {
                    construtor.Append($" {titulo[i]}");
                }
                else
                {
                    construtor.Append(titulo[i]);
                }
            }
            return construtor.ToString();
        }

        struct PropriedadeHierárquica
        {
            public string Nome { get; set; }
            public object Valor { get; set; }
        }

        struct PropriedadeLinear
        {
            public string Texto { get; set; }
            public string Complementar { get; set; }
            public int Profundidade { get; set; }
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
