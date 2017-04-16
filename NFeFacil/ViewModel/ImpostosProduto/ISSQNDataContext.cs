using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public class ISSQNDataContext : INotifyPropertyChanged, IImpostoDataContext
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ISSQN Imposto { get; } = new ISSQN();

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
