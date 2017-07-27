using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewModel.ImpostosProduto
{
    public sealed class COFINSDataContext : INotifyPropertyChanged, IImpostosUnidos
    {
        ProdutoOuServico produtoReferente;
        internal ProdutoOuServico ProdutoReferente
        {
            get => produtoReferente;
            set
            {
                produtoReferente = value;
                AtualizarImposto();
                AtualizarImpostoST();
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

        private ConjuntoCOFINS Conjunto = new ConjuntoCOFINS();
        public ConteinerCOFINS Imposto { get; set; }
        public ConteinerCOFINSST ImpostoST { get; set; }

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
            get => Conjunto.COFINS.Corpo != null ? int.Parse(Conjunto.COFINS.Corpo.CST) : -1;
            set
            {
                if (new int[] { 1, 2 }.Contains(value))
                {
                    CalculoAliquota = true;
                    CalculoValor = false;
                    Valor = true;
                    ComboTipoCalculo = false;
                    Conjunto.COFINS.Corpo = new COFINSAliq();
                }
                else if (value == 3)
                {
                    CalculoAliquota = false;
                    CalculoValor = true;
                    Valor = true;
                    ComboTipoCalculo = false;
                    Conjunto.COFINS.Corpo = new COFINSQtde();
                }
                else if (new int[] { 4, 5, 6, 7, 8, 9 }.Contains(value))
                {
                    CalculoAliquota = false;
                    CalculoValor = false;
                    Valor = false;
                    ComboTipoCalculo = false;
                    Conjunto.COFINS.Corpo = new COFINSNT();
                }
                else
                {
                    CalculoAliquota = false;
                    CalculoValor = false;
                    Valor = true;
                    ComboTipoCalculo = true;
                    Conjunto.COFINS.Corpo = new COFINSOutr();
                }
                Conjunto.COFINS.Corpo.CST = value.ToString("00");

                CalculoAliquotaST = false;
                CalculoValorST = false;
                if (value == 5)
                {
                    ValorST = true;
                    ComboTipoCalculoST = true;
                    Conjunto.COFINSST = new COFINSST();
                }
                else
                {
                    ValorST = false;
                    ComboTipoCalculoST = false;
                    Conjunto.COFINSST = null;
                }

                OnPropertyChanged(nameof(CalculoAliquota), nameof(CalculoValor), nameof(Valor),
                    nameof(ComboTipoCalculo), nameof(ComboTipoCalculoST),
                    nameof(CalculoAliquotaST), nameof(CalculoValorST), nameof(ValorST));
                AtualizarImposto();
                AtualizarImpostoST();
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
                Conjunto.COFINS.Corpo = new COFINSOutr { CST = Conjunto.COFINS.Corpo.CST };
                OnPropertyChanged(nameof(CalculoAliquota), nameof(CalculoValor));
                AtualizarImposto();
            }
        }

        private string tipoCalculoST;
        public string TipoCalculoST
        {
            get => tipoCalculoST;
            set
            {
                tipoCalculoST = value;
                Conjunto.COFINSST = new COFINSST();
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
                OnPropertyChanged(nameof(CalculoAliquotaST), nameof(CalculoValorST));
                AtualizarImpostoST();
            }
        }

        void AtualizarImposto()
        {
            Imposto = new ConteinerCOFINS(() => OnPropertyChanged(nameof(Imposto)), Conjunto.COFINS.Corpo, ProdutoReferente);
            OnPropertyChanged(nameof(Imposto));
        }

        void AtualizarImpostoST()
        {
            ImpostoST = new ConteinerCOFINSST(() => OnPropertyChanged(nameof(ImpostoST)), Conjunto.COFINSST, ProdutoReferente);
            OnPropertyChanged(nameof(ImpostoST));
        }

        public COFINSDataContext()
        {
            AtualizarImposto();
        }
        public COFINSDataContext(ConjuntoCOFINS conjunto)
        {
            if (conjunto.COFINS != null)
            {
                var corpo = conjunto.COFINS.Corpo;
                CSTSelecionado = int.Parse(corpo.CST);
                if (corpo is COFINSOutr outr)
                {
                    if (string.IsNullOrEmpty(outr.pCOFINS))
                    {
                        TipoCalculo = "Pelo valor por unidade";
                    }
                    else
                    {
                        TipoCalculo = "Por alíquota";
                    }
                }
                Conjunto.COFINS = conjunto.COFINS;

                if (conjunto.COFINSST != null)
                {
                    var st = conjunto.COFINSST;
                    if (string.IsNullOrEmpty(st.pCOFINS))
                    {
                        TipoCalculoST = "Pelo valor por unidade";
                    }
                    else
                    {
                        TipoCalculoST = "Por alíquota";
                    }
                    Conjunto.COFINSST = conjunto.COFINSST;
                    AtualizarImpostoST();
                }
            }
            AtualizarImposto();
        }

        public IEnumerable<Imposto> SepararImpostos() => Conjunto.SepararImpostos();
    }
}
