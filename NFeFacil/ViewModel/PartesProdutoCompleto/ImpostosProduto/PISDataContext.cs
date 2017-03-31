﻿using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using System.Collections.Generic;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ViewModel.PartesProdutoCompleto.ImpostosProduto
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
        public PIS Imposto
        {
            get { return Conjunto.PIS; }
        }
        public PISST ImpostoST
        {
            get { return Conjunto.PISST; }
        }

        public Visibility PIS { get; private set; } = Visibility.Collapsed;
        public Visibility CalculoAliquota { get; private set; } = Visibility.Collapsed;
        public Visibility CalculoValor { get; private set; } = Visibility.Collapsed;
        public Visibility ComboTipoCalculo { get; private set; } = Visibility.Collapsed;
        public Visibility PISST { get; private set; } = Visibility.Collapsed;
        public Visibility CalculoAliquotaST { get; private set; } = Visibility.Collapsed;
        public Visibility CalculoValorST { get; private set; } = Visibility.Collapsed;

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
                    ComboTipoCalculo = Visibility.Collapsed;
                    Conjunto.PIS = new PIS()
                    {
                        Corpo = new PISAliq()
                    };
                }
                else if (tipoPISInt == 3)
                {
                    PIS = Visibility.Visible;
                    ComboTipoCalculo = Visibility.Collapsed;
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
                    ComboTipoCalculo = Visibility.Visible;
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
                    CalculoAliquota = Visibility.Visible;
                    CalculoValor = Visibility.Collapsed;
                    break;
                case TiposCalculo.PorValor:
                    CalculoAliquota = Visibility.Collapsed;
                    CalculoValor = Visibility.Visible;
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
                        CalculoAliquotaST = Visibility.Visible;
                        CalculoValorST = Visibility.Collapsed;
                        break;
                    case TiposCalculo.PorValor:
                        CalculoAliquotaST = Visibility.Collapsed;
                        CalculoValorST = Visibility.Visible;
                        break;
                }
                OnPropertyChanged(nameof(ImpostoST), nameof(CalculoAliquotaST), nameof(CalculoValorST));
            }
        }

        public IEnumerable<Imposto> SepararImpostos() => Conjunto.SepararImpostos();

        private enum TiposCalculo
        {
            SemCalculo,
            PorAliquota,
            PorValor
        }
    }
}
