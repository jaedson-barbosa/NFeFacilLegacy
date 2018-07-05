using Windows.UI.Xaml.Media;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace BaseGeral.View
{
    public sealed class ConjuntoBasicoExibicao<T> : ConjuntoBasicoExibicao
    {
        public T Objeto { get; set; }
    }

    public class ConjuntoBasicoExibicao
    {
        public ImageSource Imagem { get; set; }
        public string Principal { get; set; }
        public string Secundario { get; set; }
    }
}
