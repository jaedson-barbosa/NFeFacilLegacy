using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

namespace NFeFacil.ViewModel
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
        public COFINS Imposto
        {
            get { return Conjunto.COFINS; }
        }
        public COFINSST ImpostoST
        {
            get { return Conjunto.COFINSST; }
        }

        public bool COFINS { get; private set; }
        public bool COFINSST { get; private set; }
        public bool ComboTipoCalculo { get; private set; }
        public bool CalculoAliquota { get; private set; }
        public bool CalculoValor { get; private set; }
        public bool CalculoAliquotaST { get; private set; }
        public bool CalculoValorST { get; private set; }

        private ComboBoxItem cstSelecionado;
        public ComboBoxItem CSTSelecionado
        {
            get { return cstSelecionado; }
            set
            {
                cstSelecionado = value;
                var tipoCOFINSString = (value.Content as string).Substring(0, 2);
                var tipoCOFINSInt = int.Parse(tipoCOFINSString);
                int[] pisAliq = { 1, 2 };
                int[] pisValor = { 3 };
                int[] pisNTrib = { 4, 5, 6, 7, 8, 9 };
                if (pisAliq.Contains(tipoCOFINSInt))
                {
                    COFINS = true;
                    MudarTpCalc(TiposCalculo.PorAliquota);
                    Imposto.Corpo = new COFINSAliq();
                    ComboTipoCalculo = false;
                }
                else if (pisValor.Contains(tipoCOFINSInt))
                {
                    COFINS = true;
                    MudarTpCalc(TiposCalculo.PorValor);
                    Imposto.Corpo = new COFINSQtde();
                    ComboTipoCalculo = false;
                }
                else if (pisNTrib.Contains(tipoCOFINSInt))
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

        private ComboBoxItem tipoCalculo;
        public ComboBoxItem TipoCalculo
        {
            get { return tipoCalculo; }
            set
            {
                tipoCalculo = value;
                MudarTpCalc(value.Content as string == "Por alíquota" ? TiposCalculo.PorAliquota : TiposCalculo.PorValor);
                Imposto.Corpo = new COFINSOutr { CST = Imposto.Corpo.CST };
                OnPropertyChanged(nameof(Imposto.Corpo));
            }
        }

        private void MudarTpCalc(TiposCalculo tipo)
        {
            switch (tipo)
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
            OnPropertyChanged(nameof(CalculoAliquota), nameof(CalculoValor));
        }

        private ComboBoxItem tipoCalculoST;
        public ComboBoxItem TipoCalculoST 
        {
            get { return tipoCalculoST; }
            set
            {
                tipoCalculoST = value;
                MudarTpCalcST(value.Content as string == "Por alíquota" ? TiposCalculo.PorAliquota : TiposCalculo.PorValor);
                Conjunto.COFINSST = new COFINSST();
                OnPropertyChanged(nameof(ImpostoST));
            }
        }

        private void MudarTpCalcST(TiposCalculo tipo)
        {
            switch (tipo)
            {
                case TiposCalculo.PorAliquota:
                    CalculoAliquotaST = true;
                    CalculoValorST = false;
                    break;
                case TiposCalculo.PorValor:
                    CalculoAliquotaST = false;
                    CalculoValorST = true;
                    break;
            }
            OnPropertyChanged(nameof(CalculoAliquotaST), nameof(CalculoValorST));
        }

        public IEnumerable<Imposto> SepararImpostos()
        {
            return Conjunto.SepararImpostos();
        }

        private enum TiposCalculo
        {
            PorAliquota,
            PorValor
        }
    }
}
