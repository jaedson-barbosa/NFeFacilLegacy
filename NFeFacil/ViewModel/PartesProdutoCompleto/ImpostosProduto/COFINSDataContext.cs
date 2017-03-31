using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewModel.PartesProdutoCompleto.ImpostosProduto
{
    public sealed class COFINSDataContext : INotifyPropertyChanged, IImpostosUnidos
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        private readonly ConjuntoCOFINS Conjunto = new ConjuntoCOFINS();
        public COFINS Imposto => Conjunto.COFINS;
        public COFINSST ImpostoST => Conjunto.COFINSST;

        public bool COFINS { get; private set; }
        public bool COFINSST { get; private set; }
        public bool ComboTipoCalculo { get; private set; }
        public bool CalculoAliquota { get; private set; }
        public bool CalculoValor { get; private set; }
        public bool CalculoAliquotaST { get; private set; }
        public bool CalculoValorST { get; private set; }

        private string cstSelecionado;
        public string CSTSelecionado
        {
            get => cstSelecionado;
            set
            {
                cstSelecionado = value;
                var tipoCOFINSString = value.Substring(0, 2);
                var tipoCOFINSInt = int.Parse(tipoCOFINSString);
                if (new int[] { 1, 2 }.Contains(tipoCOFINSInt))
                {
                    COFINS = true;
                    MudarTpCalc(TiposCalculo.PorAliquota);
                    Imposto.Corpo = new COFINSAliq();
                    ComboTipoCalculo = false;
                }
                else if (tipoCOFINSInt == 3)
                {
                    COFINS = true;
                    MudarTpCalc(TiposCalculo.PorValor);
                    Imposto.Corpo = new COFINSQtde();
                    ComboTipoCalculo = false;
                }
                else if (new int[] { 4, 5, 6, 7, 8, 9 }.Contains(tipoCOFINSInt))
                {
                    COFINS = false;
                    Imposto.Corpo = new COFINSNT();
                }
                else
                {
                    COFINS = true;
                    Imposto.Corpo = new COFINSOutr();
                    ComboTipoCalculo = true;
                }
                COFINSST = tipoCOFINSInt == 5;
                Imposto.Corpo.CST = tipoCOFINSString;
                OnPropertyChanged(nameof(COFINS), nameof(Imposto.Corpo), nameof(ComboTipoCalculo), nameof(COFINSST));
            }
        }

        private string tipoCalculo;
        public string TipoCalculo
        {
            get => tipoCalculo;
            set
            {
                tipoCalculo = value;
                MudarTpCalc(value == "Por alíquota" ? TiposCalculo.PorAliquota : TiposCalculo.PorValor);
                Imposto.Corpo = new COFINSOutr { CST = Imposto.Corpo.CST };
                OnPropertyChanged(nameof(Imposto.Corpo));
            }
        }

        private void MudarTpCalc(TiposCalculo tipo)
        {
            CalculoValor = !(CalculoAliquota = tipo == TiposCalculo.PorAliquota);
            OnPropertyChanged(nameof(CalculoAliquota), nameof(CalculoValor));
        }

        private string tipoCalculoST;
        public string TipoCalculoST 
        {
            get => tipoCalculoST;
            set
            {
                tipoCalculoST = value;
                MudarTpCalcST(value == "Por alíquota" ? TiposCalculo.PorAliquota : TiposCalculo.PorValor);
                Conjunto.COFINSST = new COFINSST();
                OnPropertyChanged(nameof(ImpostoST));
            }
        }

        private void MudarTpCalcST(TiposCalculo tipo)
        {
            CalculoValorST = !(CalculoAliquotaST = tipo == TiposCalculo.PorAliquota);
            OnPropertyChanged(nameof(CalculoAliquotaST), nameof(CalculoValorST));
        }

        public IEnumerable<Imposto> SepararImpostos() => Conjunto.SepararImpostos();

        private enum TiposCalculo
        {
            PorAliquota,
            PorValor
        }
    }
}
