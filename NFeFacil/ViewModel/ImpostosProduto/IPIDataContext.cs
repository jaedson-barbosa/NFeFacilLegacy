using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;
using System.Linq;

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

        public bool CalculoAliquota { get; private set; }
        public bool CalculoValor { get; private set; }
        public bool Valor { get; private set; }
        public bool ComboTipoCalculo { get; private set; }

        public int CSTSelecionado
        {
            get => Imposto.Corpo != null ? int.Parse(Imposto.Corpo.CST) : -1;
            set
            {
                if (new int[] { 0, 49, 50, 99 }.Contains(value))
                {
                    Imposto.Corpo = new IPITrib();
                    Valor = ComboTipoCalculo = true;
                    CalculoAliquota = CalculoValor = false;
                }
                else
                {
                    Imposto.Corpo = new IPINT();
                    Valor = ComboTipoCalculo = CalculoAliquota = CalculoValor = false;
                }
                Imposto.Corpo.CST = value.ToString("F2");
                OnPropertyChanged(nameof(Imposto), nameof(Valor), nameof(ComboTipoCalculo));
            }
        }

        private string tipoCalculo;
        public string TipoCalculo
        {
            get => tipoCalculo;
            set
            {
                tipoCalculo = value;
                CalculoValor = !(CalculoAliquota = value == "Por alíquota");
                OnPropertyChanged(nameof(CalculoAliquota), nameof(CalculoValor));
            }
        }

        public Imposto ImpostoBruto => Imposto;
    }
}
