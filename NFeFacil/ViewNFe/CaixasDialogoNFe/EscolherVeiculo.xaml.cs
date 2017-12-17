using NFeFacil.ItensBD;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoNFe
{
    public sealed partial class EscolherVeiculo : ContentDialog
    {
        ObservableCollection<VeiculoDI> Veiculos { get; }
        public VeiculoDI Escolhido { get; }

        public EscolherVeiculo(VeiculoDI[] veiculos, VeiculoDI escolhido)
        {
            Veiculos = veiculos.GerarObs();
            Escolhido = escolhido;
            InitializeComponent();
        }
    }
}
