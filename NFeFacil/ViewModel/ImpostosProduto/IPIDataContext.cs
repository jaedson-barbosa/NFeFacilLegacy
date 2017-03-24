using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class IPIDataContext : INotifyPropertyChanged, IImpostoDataContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        public IPI Imposto { get; } = new IPI();

        public bool VisibilidadeIPI { get; private set; }
        public bool CalculoAliquota { get; private set; }
        public bool CalculoValor { get; private set; }

        private ComboBoxItem cstSelecionado;
        public ComboBoxItem CSTSelecionado
        {
            get { return cstSelecionado; }
            set
            {
                cstSelecionado = value;
                var tipoIPI = value.Content as string;
                var tipoIPIString = tipoIPI.Substring(0, 2);
                int[] ipiTrib = { 0, 49, 50, 99 };
                if (ipiTrib.Contains(int.Parse(tipoIPIString)))
                {
                    Imposto.Corpo = new IPITrib();
                    VisibilidadeIPI = true;
                }
                else
                {
                    Imposto.Corpo = new IPINT();
                    VisibilidadeIPI = false;
                }
                Imposto.Corpo.CST = tipoIPIString;
                OnPropertyChanged(nameof(Imposto.Corpo), nameof(VisibilidadeIPI));
            }
        }

        private ComboBoxItem tipoCalculo;
        public ComboBoxItem TipoCalculo
        {
            get { return tipoCalculo; }
            set
            {
                tipoCalculo = value;
                if (value.Content as string == "Por alíquota")
                {
                    CalculoAliquota = true;
                    CalculoValor = false;
                }
                else
                {
                    CalculoAliquota = true;
                    CalculoValor = false;
                }
                OnPropertyChanged(nameof(CalculoAliquota), nameof(CalculoValor));
            }
        }

        public Imposto ImpostoBruto => Imposto;
    }
}
