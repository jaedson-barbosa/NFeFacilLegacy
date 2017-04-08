using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    [PropertyChanged.ImplementPropertyChanged]
    public sealed partial class Testes : Page
    {
        public enderecoCompleto Objeto { get; set; } = new enderecoCompleto();

        public Testes()
        {
            this.InitializeComponent();
            DataContext = this;
        }
    }
}
