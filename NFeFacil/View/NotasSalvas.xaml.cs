using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using NFeFacil.ViewModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class NotasSalvas : Page, IEsconde
    {
        public NotasSalvas()
        {
            InitializeComponent();
            Propriedades.Intercambio.SeAtualizar(Telas.NotasSalvas, Symbol.Library, "Notas salvas");
            DataContext = new NotasSalvasDataContext(ref lstNotas);
        }

        public async Task EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }
    }
}
