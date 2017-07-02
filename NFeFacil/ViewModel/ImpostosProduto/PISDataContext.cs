﻿using System.ComponentModel;
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

        public bool CalculoAliquota { get; private set; }
        public bool CalculoValor { get; private set; }
        public bool Valor { get; private set; }
        public bool ComboTipoCalculo { get; private set; }

        public bool CalculoAliquotaST { get; private set; }
        public bool CalculoValorST { get; private set; }
        public bool ValorST { get; private set; }
        public bool ComboTipoCalculoST { get; private set; }

        public int CSTSelecionado
        {
            get => Imposto.Corpo != null ? int.Parse(Imposto.Corpo.CST) : -1;
            set
            {
                if (new int[] { 1, 2 }.Contains(value))
                {
                    CalculoAliquota = true;
                    CalculoValor = false;
                    Valor = true;
                    ComboTipoCalculo = false;
                    Conjunto.PIS = new PIS()
                    {
                        Corpo = new PISAliq()
                    };
                }
                else if (value == 3)
                {
                    CalculoAliquota = false;
                    CalculoValor = true;
                    Valor = true;
                    ComboTipoCalculo = false;
                    Conjunto.PIS = new PIS()
                    {
                        Corpo = new PISQtde()
                    };
                }
                else if (new int[] { 4, 5, 6, 7, 8, 9 }.Contains(value))
                {
                    CalculoAliquota = false;
                    CalculoValor = false;
                    Valor = false;
                    ComboTipoCalculo = false;
                    Conjunto.PIS = new PIS()
                    {
                        Corpo = new PISNT()
                    };
                }
                else
                {
                    CalculoAliquota = false;
                    CalculoValor = false;
                    Valor = true;
                    ComboTipoCalculo = true;
                    Conjunto.PIS = new PIS()
                    {
                        Corpo = new PISOutr()
                    };
                }
                Imposto.Corpo.CST = value.ToString("00");

                CalculoAliquotaST = false;
                CalculoValorST = false;
                if (value == 5)
                {
                    ValorST = true;
                    ComboTipoCalculoST = true;
                    Conjunto.PISST = new PISST();
                }
                else
                {
                    ValorST = false;
                    ComboTipoCalculoST = false;
                    Conjunto.PISST = null;
                }

                OnPropertyChanged(nameof(CalculoAliquota), nameof(CalculoValor), nameof(Valor),
                    nameof(ComboTipoCalculo), nameof(ComboTipoCalculoST),
                    nameof(CalculoAliquotaST), nameof(CalculoValorST), nameof(ValorST),
                    nameof(Imposto), nameof(ImpostoST));
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
                Imposto.Corpo = new PISOutr { CST = Imposto.Corpo.CST };
                OnPropertyChanged(nameof(Imposto), nameof(CalculoAliquota), nameof(CalculoValor));
            }
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
