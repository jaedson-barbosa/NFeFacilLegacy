using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    public sealed partial class EscolhaImposto : ContentDialog
    {
        internal EscolhaImposto(List<PrincipaisImpostos> adicionaveis)
        {
            this.InitializeComponent();
            Impostos = adicionaveis.Select(x => new VisualizacaoComboBox(x)).GerarObs();
        }

        ObservableCollection<VisualizacaoComboBox> Impostos { get; }
        internal PrincipaisImpostos Escolhido { get; private set; }

        struct VisualizacaoComboBox
        {
            public string Nome { get; set; }
            public PrincipaisImpostos Primitivo { get; set; }

            public VisualizacaoComboBox(PrincipaisImpostos imposto)
            {
                switch (imposto)
                {
                    case PrincipaisImpostos.ICMS:
                        Nome = "ICMS";
                        break;
                    case PrincipaisImpostos.IPI:
                        Nome = "IPI";
                        break;
                    case PrincipaisImpostos.II:
                        Nome = "Imposto de importação";
                        break;
                    case PrincipaisImpostos.ISSQN:
                        Nome = "ISSQN";
                        break;
                    case PrincipaisImpostos.PIS:
                        Nome = "PIS";
                        break;
                    case PrincipaisImpostos.COFINS:
                        Nome = "COFINS";
                        break;
                    case PrincipaisImpostos.ICMSUFDest:
                        Nome = "ICMS de destino";
                        break;
                    default:
                        throw new System.Exception();
                }
                Primitivo = imposto;
            }
        }
    }
}
