using NFeFacil.IBGE;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace NFeFacil.ViewModel.PartesProdutoCompleto.ImpostosProduto
{
    public class ISSQNDataContext : INotifyPropertyChanged, IImpostoDataContext
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ISSQN Imposto { get; } = new ISSQN();

        public int ExigISS
        {
            get => int.Parse(Imposto.indISS ?? "0") - 1;
            set => Imposto.indISS = (value + 1).ToString();
        }

        public int Incent
        {
            get => int.Parse(Imposto.indIncentivo ?? "0") - 1;
            set => Imposto.indIncentivo = (value + 1).ToString();
        }

        private bool exterior;
        public bool Exterior
        {
            get => exterior;
            set
            {
                if (exterior = value)
                {
                    VisibilidadeCodigoPais = Visibility.Visible;
                    VisibilidadeMunicipioUFIncidencia = Visibility.Collapsed;
                    Imposto.cMun = "9999999";
                }
                else
                {
                    VisibilidadeCodigoPais = Visibility.Collapsed;
                    VisibilidadeMunicipioUFIncidencia = Visibility.Visible;
                    Imposto.cPais = null;
                    Imposto.cMun = null;
                }
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(VisibilidadeCodigoPais)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(VisibilidadeMunicipioUFIncidencia)));
            }
        }

        public Visibility VisibilidadeCodigoPais { get; private set; } = Visibility.Collapsed;
        public Visibility VisibilidadeMunicipioUFIncidencia { get; private set; }

        public Imposto ImpostoBruto => Imposto;
    }
}
