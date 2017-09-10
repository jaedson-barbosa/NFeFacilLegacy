using NFeFacil.ModeloXML.PartesProcesso;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static NFeFacil.ExtensoesPrincipal;

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
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NotaFiscal = (NFe)e.Parameter;
            var propriedades = ObterPropriedades(NotaFiscal);
        }

        List<Propriedade> ObterPropriedades(object obj)
        {
            var retorno = new List<Propriedade>();
            var tipo = obj.GetType();
            foreach (var prop in tipo.GetProperties())
            {
                var valor = prop.GetValue(obj);
                if (valor != null)
                {
                    var tipoFilho = valor.GetType();
                    if (tipoFilho.Namespace.Contains("NFeFacil") && !tipoFilho.IsArray)
                    {
                        retorno.Add(new Propriedade
                        {
                            Nome = prop.Name,
                            Valor = ObterPropriedades(valor)
                        });
                    }
                    else if (tipoFilho.Namespace.Contains("NFeFacil"))
                    {
                        var teste = (IEnumerable)valor;
                        List<Propriedade> propriedadesFilhas = new List<Propriedade>();
                        int i = 1;
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
                            propriedadesFilhas.Add(new Propriedade
                            {
                                Nome = $"Item {i++}",
                                Valor = valorItem
                            });
                        }
                        retorno.Add(new Propriedade
                        {
                            Nome = prop.Name,
                            Valor = propriedadesFilhas
                        });
                    }
                    else
                    {
                        retorno.Add(new Propriedade
                        {
                            Nome = prop.Name,
                            Valor = valor
                        });
                    }
                }
            }
            return retorno;
        }

        struct Propriedade
        {
            public string Nome { get; set; }
            public object Valor { get; set; }
        }
    }
}
