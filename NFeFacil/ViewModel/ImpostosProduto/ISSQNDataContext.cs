using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;

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
                    VisibilidadeCodigoPais = true;
                    VisibilidadeMunicipioUFIncidencia = false;
                    Imposto.cMun = "9999999";
                }
                else
                {
                    VisibilidadeCodigoPais = false;
                    VisibilidadeMunicipioUFIncidencia = true;
                    Imposto.cPais = null;
                    Imposto.cMun = null;
                }
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(VisibilidadeCodigoPais)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(VisibilidadeMunicipioUFIncidencia)));
            }
        }

        public bool VisibilidadeCodigoPais { get; private set; }
        public bool VisibilidadeMunicipioUFIncidencia { get; private set; } = true;

        public Imposto ImpostoBruto => Imposto;
    }
}
