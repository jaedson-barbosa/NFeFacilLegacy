using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using System.Collections.Generic;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class PISDataContext : INotifyPropertyChanged, IImpostosUnidos
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        private ConjuntoPIS Conjunto = new ConjuntoPIS();
        public PIS Imposto => Conjunto.PIS;
        public PISST ImpostoST => Conjunto.PISST;

        public Visibility PIS { get; private set; } = Visibility.Collapsed;
        public bool CalculoAliquota { get; private set; }
        public bool CalculoValor { get; private set; }
        public bool ComboTipoCalculo { get; private set; }
        public Visibility PISST { get; private set; } = Visibility.Collapsed;
        public bool CalculoAliquotaST { get; private set; }
        public bool CalculoValorST { get; private set; }

        private string cstSelecionado;
        public string CSTSelecionado
        {
            get => cstSelecionado;
            set
            {
                cstSelecionado = value;
                var tipoPISString = value.Substring(0, 2);
                var tipoPISInt = int.Parse(tipoPISString);
                if (new int[] { 1, 2 }.Contains(tipoPISInt))
                {
                    MudarTipoCalculo(TiposCalculo.PorAliquota);
                    PIS = Visibility.Visible;
                    ComboTipoCalculo = false;
                    Conjunto.PIS = new PIS()
                    {
                        Corpo = new PISAliq()
                    };
                }
                else if (tipoPISInt == 3)
                {
                    PIS = Visibility.Visible;
                    ComboTipoCalculo = false;
                    MudarTipoCalculo(TiposCalculo.PorValor);
                    Conjunto.PIS = new PIS()
                    {
                        Corpo = new PISQtde()
                    };
                }
                else if (new int[] { 4, 5, 6, 7, 8, 9 }.Contains(tipoPISInt))
                {
                    PIS = Visibility.Collapsed;
                    Conjunto.PIS = new PIS()
                    {
                        Corpo = new PISNT()
                    };
                }
                else
                {
                    PIS = Visibility.Visible;
                    ComboTipoCalculo = false;
                    Conjunto.PIS = new PIS()
                    {
                        Corpo = new PISOutr()
                    };
                }
                PISST = tipoPISInt == 5 ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(PIS), nameof(PISST), nameof(ComboTipoCalculo), nameof(Imposto));
                Imposto.Corpo.CST = tipoPISString;
            }
        }

        private string tipoCalculo;
        public string TipoCalculo
        {
            get => tipoCalculo;
            set
            {
                tipoCalculo = value;
                MudarTipoCalculo(value == "Por alíquota" ? TiposCalculo.PorAliquota : TiposCalculo.PorValor);
                Imposto.Corpo = new PISOutr { CST = Imposto.Corpo.CST };
                OnPropertyChanged(nameof(Imposto));
            }
        }

        private void MudarTipoCalculo(TiposCalculo tipo)
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

        private string tipoCalculoST;
        public string TipoCalculoST
        {
            get => tipoCalculoST;
            set
            {
                tipoCalculoST = value;
                Conjunto.PISST = new PISST();
                switch (value == "Por alíquota" ? TiposCalculo.PorAliquota : TiposCalculo.PorValor)
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
                OnPropertyChanged(nameof(ImpostoST), nameof(CalculoAliquotaST), nameof(CalculoValorST));
            }
        }

        public IEnumerable<Imposto> SepararImpostos() => Conjunto.SepararImpostos();

        private enum TiposCalculo
        {
            PorAliquota,
            PorValor
        }
    }
}
