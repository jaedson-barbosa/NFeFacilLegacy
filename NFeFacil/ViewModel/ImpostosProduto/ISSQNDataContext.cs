using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public class ISSQNDataContext : INotifyPropertyChanged, IImpostoDataContext
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

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
                OnPropertyChanged(nameof(VisibilidadeCodigoPais), nameof(VisibilidadeMunicipioUFIncidencia));
            }
        }

        public bool VisibilidadeCodigoPais { get; private set; }
        public bool VisibilidadeMunicipioUFIncidencia { get; private set; } = true;

        public ISSQNDataContext() { }
        public ISSQNDataContext(ISSQN imposto)
        {
            Imposto = imposto;
            Exterior = imposto.cMun == "9999999";
            OnPropertyChanged(nameof(Exterior));
        }

        public Imposto ImpostoBruto => Imposto;
    }
}
