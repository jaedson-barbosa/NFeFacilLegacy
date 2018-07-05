using BaseGeral;
using BaseGeral.ItensBD;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Comum.CaixasDialogo
{
    public sealed partial class EscolherVeiculo : ContentDialog
    {
        ObservableCollection<VeiculoDI> Veiculos { get; }
        public VeiculoDI Escolhido { get; private set; }

        public EscolherVeiculo(VeiculoDI[] veiculos, VeiculoDI escolhido)
        {
            Veiculos = veiculos.GerarObs();
            Escolhido = escolhido;
            InitializeComponent();
        }
    }
}
