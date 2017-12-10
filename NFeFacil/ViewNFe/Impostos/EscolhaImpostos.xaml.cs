using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ViewNFe.CaixasImpostos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class EscolhaImpostos : Page
    {
        ObservableCollection<string> impostos;
        ObservableCollection<string> Impostos
        {
            get => impostos;
            set
            {
                impostos = value;
                InitializeComponent();
            }
        }

        List<IDetalhamentoImposto> Escolhidos { get; set; } = new List<IDetalhamentoImposto>();

        DetalhesProdutos ProdutoCompleto;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ProdutoCompleto = (DetalhesProdutos)e.Parameter;

            var caixa = new MessageDialog("Qual o tipo de imposto que é usado neste dado?", "Entrada");
            caixa.Commands.Add(new UICommand("ICMS"));
            caixa.Commands.Add(new UICommand("ISSQN"));
            if ((await caixa.ShowAsync()).Label == "ICMS")
            {
                Impostos = new ObservableCollection<string>
                {
                    "ICMS",
                    "IPI",
                    "PIS",
                    "COFINS",
                    "II",
                    "ICMSUFDest"
                };
            }
            else
            {
                Impostos = new ObservableCollection<string>
                {
                    "IPI",
                    "PIS",
                    "COFINS",
                    "ISSQN",
                    "ICMSUFDest"
                };
            }
        }

        async void Grd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var input = (GridView)sender;
            var quantAdicionada = e.AddedItems.Count;
            var quantRemovida = e.RemovedItems.Count;
            if (quantRemovida > 0)
            {
                for (int i = 0; i < quantRemovida; i++)
                {
                    var item = (string)e.RemovedItems[i];
                    var imposto = (PrincipaisImpostos)Enum.Parse(typeof(PrincipaisImpostos), item);
                    switch (imposto)
                    {
                        case PrincipaisImpostos.ICMS:
                            Escolhidos.RemoveAll(x => x is DetalhamentoICMS.Detalhamento);
                            break;
                        case PrincipaisImpostos.IPI:
                            Escolhidos.RemoveAll(x => x is DetalhamentoIPI.Detalhamento);
                            break;
                        case PrincipaisImpostos.II:
                            Escolhidos.RemoveAll(x => x is DetalhamentoII.Detalhamento);
                            break;
                        case PrincipaisImpostos.ISSQN:
                            Escolhidos.RemoveAll(x => x is DetalhamentoISSQN.Detalhamento);
                            break;
                        case PrincipaisImpostos.PIS:
                            Escolhidos.RemoveAll(x => x is DetalhamentoPIS.Detalhamento);
                            break;
                        case PrincipaisImpostos.COFINS:
                            Escolhidos.RemoveAll(x => x is DetalhamentoCOFINS.Detalhamento);
                            break;
                        case PrincipaisImpostos.ICMSUFDest:
                            Escolhidos.RemoveAll(x => x is DetalhamentoICMSUFDest.Detalhamento);
                            break;
                    }
                }
            }
            if (quantAdicionada > 0)
            {
                for (int i = 0; i < quantAdicionada; i++)
                {
                    var item = (string)e.AddedItems[i];
                    var imposto = (PrincipaisImpostos)Enum.Parse(typeof(PrincipaisImpostos), item);
                    bool sucesso = true;
                    switch (imposto)
                    {
                        case PrincipaisImpostos.ICMS:
                            sucesso = await DetalharICMS();
                            break;
                        case PrincipaisImpostos.IPI:
                            sucesso = await DetalharIPI();
                            break;
                        case PrincipaisImpostos.II:
                            Escolhidos.Add(new DetalhamentoII.Detalhamento());
                            break;
                        case PrincipaisImpostos.ISSQN:
                            await DetalharISSQN();
                            break;
                        case PrincipaisImpostos.PIS:
                            sucesso = await DetalharPIS();
                            break;
                        case PrincipaisImpostos.COFINS:
                            sucesso = await DetalharCOFINS();
                            break;
                        case PrincipaisImpostos.ICMSUFDest:
                            Escolhidos.Add(new DetalhamentoICMSUFDest.Detalhamento());
                            break;
                    }
                    if (!sucesso) input.SelectedItems.Remove(item);
                }
            }
        }

        async Task<bool> DetalharICMS()
        {
            var caixa = new EscolherTipoICMS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Escolhidos.Add(new DetalhamentoICMS.Detalhamento
                {
                    Origem = caixa.Origem,
                    Regime = caixa.Regime,
                    TipoICMSRN = caixa.TipoICMSRN,
                    TipoICMSSN = caixa.TipoICMSSN
                });
                return true;
            }
            return false;
        }

        async Task<bool> DetalharIPI()
        {
            var caixa = new EscolherTipoIPI();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Escolhidos.Add(new DetalhamentoIPI.Detalhamento
                {
                    CST = int.Parse(caixa.CST),
                    TipoCalculo = caixa.TipoCalculo
                });
                return true;
            }
            return false;
        }

        async Task DetalharISSQN()
        {
            var caixa = new MessageDialog("Qual o tipo de ISSQN desejado?", "Entrada");
            caixa.Commands.Add(new UICommand("Nacional"));
            caixa.Commands.Add(new UICommand("Exterior"));
            Escolhidos.Add(new DetalhamentoISSQN.Detalhamento
            {
                Exterior = (await caixa.ShowAsync()).Label == "Exterior"
            });
        }

        async Task<bool> DetalharPIS()
        {
            var caixa = new EscolherTipoPISouCOFINS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Escolhidos.Add(new DetalhamentoPIS.Detalhamento
                {
                    CST = int.Parse(caixa.CST),
                    TipoCalculo = caixa.TipoCalculo,
                    TipoCalculoST = caixa.TipoCalculoST
                });
                return true;
            }
            return false;
        }

        async Task<bool> DetalharCOFINS()
        {
            var caixa = new EscolherTipoPISouCOFINS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Escolhidos.Add(new DetalhamentoCOFINS.Detalhamento
                {
                    CST = int.Parse(caixa.CST),
                    TipoCalculo = caixa.TipoCalculo,
                    TipoCalculoST = caixa.TipoCalculoST
                });
                return true;
            }
            return false;
        }

        private void Avancar(object sender, RoutedEventArgs e)
        {
            var roteiro = new RoteiroAdicaoImpostos(Escolhidos, ProdutoCompleto);
            MainPage.Current.Navegar<DetalhamentoGeral>(roteiro);
        }
    }
}
