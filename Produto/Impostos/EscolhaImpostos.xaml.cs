using BaseGeral;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos
{
    [DetalhePagina("\uE825", "Impostos")]
    public sealed partial class EscolhaImpostos : Page, INotifyPropertyChanged
    {
        ImpostoArmazenado[] OpcoesICMS;
        ImpostoArmazenado[] OpcoesCOFINS;
        ImpostoArmazenado[] OpcoesPIS;
        ImpostoArmazenado[] OpcoesIPI;
        ImpostoArmazenado[] OpcoesICMSUFDest;

        bool TaxarIPI
        {
            get => IPIEscolhido != null;
            set
            {
                cmbIPI.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                cmbIPI.SelectedIndex = -1;
            }
        }

        bool TaxarICMSUFDest
        {
            get => false;
            set
            {
                if (value) Escolhidos.Add(new DetalhamentoICMSUFDest.Detalhamento());
                else RemoverImpostoEscolhido(PrincipaisImpostos.ICMSUFDest);
            }
        }

        ImpostoArmazenado _ICMSEscolhido;
        ImpostoArmazenado ICMSEscolhido
        {
            get
            {
                if (_ICMSEscolhido == null && ImpostosPadrao.Any(x => x.Tipo == PrincipaisImpostos.ICMS))
                {
                    var (Tipo, NomeTemplate, CST) = ImpostosPadrao.First(x => x.Tipo == PrincipaisImpostos.ICMS);
                    _ICMSEscolhido = OpcoesICMS.First(x => x.NomeTemplate == NomeTemplate && x.CST == CST);
                }
                return _ICMSEscolhido;
            }
            set
            {
                if (value != null)
                {
                    RemoverImpostoEscolhido(PrincipaisImpostos.ICMS);
                    if (value is ICMSArmazenado armazenado)
                        Escolhidos.Add(armazenado);
                    else DetalharICMS(value);
                }
            }
        }

        ImpostoArmazenado _COFINSEscolhido;
        ImpostoArmazenado COFINSEscolhido
        {
            get
            {
                if (_COFINSEscolhido == null && ImpostosPadrao.Any(x => x.Tipo == PrincipaisImpostos.COFINS))
                {
                    var (Tipo, NomeTemplate, CST) = ImpostosPadrao.First(x => x.Tipo == PrincipaisImpostos.COFINS);
                    _COFINSEscolhido = OpcoesCOFINS.First(x => x.NomeTemplate == NomeTemplate && x.CST == CST);
                }
                return _COFINSEscolhido;
            }
            set
            {
                if (value != null)
                {
                    RemoverImpostoEscolhido(PrincipaisImpostos.COFINS);
                    if (value is ImpSimplesArmazenado armazenado)
                        Escolhidos.Add(armazenado);
                    else DetalharCOFINS(value);
                }
            }
        }

        ImpostoArmazenado _PISEscolhido;
        ImpostoArmazenado PISEscolhido
        {
            get
            {
                if (_PISEscolhido == null && ImpostosPadrao.Any(x => x.Tipo == PrincipaisImpostos.PIS))
                {
                    var (Tipo, NomeTemplate, CST) = ImpostosPadrao.First(x => x.Tipo == PrincipaisImpostos.PIS);
                    _PISEscolhido = OpcoesPIS.First(x => x.NomeTemplate == NomeTemplate && x.CST == CST);
                }
                return _PISEscolhido;
            }
            set
            {
                if (value != null)
                {
                    RemoverImpostoEscolhido(PrincipaisImpostos.PIS);
                    if (value is ImpSimplesArmazenado armazenado)
                        Escolhidos.Add(armazenado);
                    else DetalharPIS(value);
                }
            }
        }

        ImpostoArmazenado _IPIEscolhido;
        ImpostoArmazenado IPIEscolhido
        {
            get
            {
                if (_IPIEscolhido == null && ImpostosPadrao.Any(x => x.Tipo == PrincipaisImpostos.IPI))
                {
                    var (Tipo, NomeTemplate, CST) = ImpostosPadrao.First(x => x.Tipo == PrincipaisImpostos.IPI);
                    _IPIEscolhido = OpcoesIPI.First(x => x.NomeTemplate == NomeTemplate && x.CST == CST);
                }
                return _IPIEscolhido;
            }
            set
            {
                if (value != null)
                {
                    RemoverImpostoEscolhido(PrincipaisImpostos.IPI);
                    if (value is ImpSimplesArmazenado armazenado)
                        Escolhidos.Add(armazenado);
                    else DetalharIPI(value);
                }
            }
        }

        List<IDetalhamentoImposto> Escolhidos { get; set; } = new List<IDetalhamentoImposto>();

        DetalhesProdutos ProdutoCompleto;
        (PrincipaisImpostos Tipo, string NomeTemplate, int CST)[] ImpostosPadrao;

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var conjunto = (DadosAdicaoProduto)e.Parameter;
            ProdutoCompleto = conjunto.Completo;
            ImpostosPadrao = conjunto.ImpostosPadrao;

            var impostos = conjunto.IsNFCe ? conjunto.GetImpostosPadraoNFCe() : conjunto.GetImpostosPadraoNFe();
            OpcoesICMS = impostos.Where(x => x.Tipo == PrincipaisImpostos.ICMS).ToArray();
            OpcoesCOFINS = impostos.Where(x => x.Tipo == PrincipaisImpostos.COFINS).ToArray();
            OpcoesPIS = impostos.Where(x => x.Tipo == PrincipaisImpostos.PIS).ToArray();
            OpcoesIPI = impostos.Where(x => x.Tipo == PrincipaisImpostos.IPI).ToArray();
            OpcoesICMSUFDest = impostos.Where(x => x.Tipo == PrincipaisImpostos.ICMSUFDest).ToArray();
            InitializeComponent();
        }

        async void DetalharICMS(ImpostoArmazenado novo)
        {
            var caixa = new EscolherTipoICMS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Escolhidos.Add(new DetalhamentoICMS.Detalhamento
                {
                    Origem = caixa.Origem,
                    TipoICMSRN = caixa.TipoICMSRN,
                    TipoICMSSN = caixa.TipoICMSSN
                });
                _ICMSEscolhido = novo;
            }
            else
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ICMSEscolhido)));
        }

        async void DetalharIPI(ImpostoArmazenado novo)
        {
            var caixa = new EscolherTipoIPI();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Escolhidos.Add(new DetalhamentoIPI.Detalhamento
                {
                    CST = int.Parse(caixa.CST),
                    TipoCalculo = caixa.TipoCalculo
                });
                _IPIEscolhido = novo;
            }
            else
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IPIEscolhido)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TaxarIPI)));
            }
        }

        async void DetalharPIS(ImpostoArmazenado novo)
        {
            var caixa = new EscolherTipoPISouCOFINS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Escolhidos.Add(new DetalhamentoPIS.Detalhamento
                {
                    CST = int.Parse(caixa.CST),
                    TipoCalculo = caixa.TipoCalculo,
                });
                _PISEscolhido = novo;
            }
            else
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(PISEscolhido)));
        }

        async void DetalharCOFINS(ImpostoArmazenado novo)
        {
            var caixa = new EscolherTipoPISouCOFINS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Escolhidos.Add(new DetalhamentoCOFINS.Detalhamento
                {
                    CST = int.Parse(caixa.CST),
                    TipoCalculo = caixa.TipoCalculo,
                });
                _COFINSEscolhido = novo;
            }
            else
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(COFINSEscolhido)));
        }

        void Avancar(object sender, RoutedEventArgs e)
        {
            var roteiro = new RoteiroAdicaoImpostos(Escolhidos.ToArray(), ProdutoCompleto);
            BasicMainPage.Current.Navegar<DetalhamentoGeral>(roteiro);
        }

        void RemoverImpostoEscolhido(PrincipaisImpostos tipo)
        {
            if (Escolhidos.Any(x => x.Tipo == tipo))
                Escolhidos.RemoveAll(x => x.Tipo == tipo);
        }
    }
}
