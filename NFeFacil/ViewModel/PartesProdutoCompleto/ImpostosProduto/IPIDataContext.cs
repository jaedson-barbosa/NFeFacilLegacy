using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;
using System.Linq;

namespace NFeFacil.ViewModel.PartesProdutoCompleto.ImpostosProduto
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

        private string cstSelecionado;
        public string CSTSelecionado
        {
            get => cstSelecionado; set
            {
                cstSelecionado = value;
                var tipoIPIString = value.Substring(0, 2);
                if (new int[] { 0, 49, 50, 99 }.Contains(int.Parse(tipoIPIString)))
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
