using NFeFacil.ModeloXML.PartesDetalhes.PartesTransporte;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item da Caixa de Diálogo de Conteúdo está documentado em http://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoNFe
{
    public sealed partial class AdicionarVolume : ContentDialog
    {
        public Volume Contexto { get; } = new Volume();
        ObservableCollection<Lacre> Lacres { get; } = new ObservableCollection<Lacre>();

        public AdicionarVolume()
        {
            InitializeComponent();
        }

        private void AdicionarLacre(object sender, RoutedEventArgs e)
        {
            var novo = new Lacre { NLacre = intLacre.Text };
            Contexto.Lacres.Add(novo);
            Lacres.Add(novo);
            intLacre.Text = "";
        }

        private void DeletarLacre(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var lacre = (Lacre)contexto;
            var index = Lacres.IndexOf(lacre);

            Contexto.Lacres.RemoveAt(index);
            Lacres.RemoveAt(index);
        }
    }
}
