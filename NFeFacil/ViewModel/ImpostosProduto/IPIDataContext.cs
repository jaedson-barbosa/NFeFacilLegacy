using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.ComponentModel;
using System.Linq;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class IPIDataContext : INotifyPropertyChanged, IImpostoDataContext
    {
        ProdutoOuServico produtoReferente;
        internal ProdutoOuServico ProdutoReferente
        {
            get => produtoReferente;
            set
            {
                produtoReferente = value;
                AtualizarImposto();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        public IPI Conjunto { get; } = new IPI();
        public ConteinerIPI Imposto { get; set; }

        public bool CalculoAliquota { get; private set; }
        public bool CalculoValor { get; private set; }
        public bool Valor { get; private set; }
        public bool ComboTipoCalculo { get; private set; }

        public int CSTSelecionado
        {
            get => Conjunto.Corpo != null ? int.Parse(Conjunto.Corpo.CST) : -1;
            set
            {
                if (new int[] { 0, 49, 50, 99 }.Contains(value))
                {
                    Conjunto.Corpo = new IPITrib();
                    Valor = ComboTipoCalculo = true;
                    CalculoAliquota = CalculoValor = false;
                }
                else
                {
                    Conjunto.Corpo = new IPINT();
                    Valor = ComboTipoCalculo = CalculoAliquota = CalculoValor = false;
                }
                Conjunto.Corpo.CST = value.ToString("00");
                OnPropertyChanged(nameof(Imposto), nameof(Valor), nameof(ComboTipoCalculo),
                    nameof(CalculoAliquota), nameof(CalculoValor));
            }
        }

        private string tipoCalculo;
        public string TipoCalculo
        {
            get => tipoCalculo;
            set
            {
                tipoCalculo = value;
                switch (value == "Por alíquota" ? TiposCalculo.PorAliquota : TiposCalculo.PorValor)
                {
                    case TiposCalculo.PorAliquota:
                        CalculoAliquota = true;
                        CalculoValor = false;
                        break;
                    case TiposCalculo.PorValor:
                        CalculoAliquota = false;
                        CalculoValor = true;
                        break;
                }
                Conjunto.Corpo = new IPITrib() { CST = Conjunto.Corpo.CST };
                OnPropertyChanged(nameof(CalculoAliquota), nameof(CalculoValor));
                AtualizarImposto();
            }
        }

        void AtualizarImposto()
        {
            Imposto = new ConteinerIPI(() => OnPropertyChanged(nameof(Imposto)), Conjunto.Corpo, ProdutoReferente);
            OnPropertyChanged(nameof(Imposto));
        }

        public Imposto ImpostoBruto => Conjunto;
    }
}
