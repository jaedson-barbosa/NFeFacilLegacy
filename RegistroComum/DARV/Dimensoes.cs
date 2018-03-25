using static BaseGeral.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum.DARV
{
    public struct Dimensoes
    {
        public Dimensoes(double largura, double altura, double padding)
        {
            LarguraOriginal = largura;
            LarguraProcessada = CMToPixel(largura);
            AlturaOriginal = altura;
            AlturaProcessada = altura != 0 ? CMToPixel(altura) : double.NaN;
            PaddingOriginal = padding;
            PaddingProcessado = CMToPixel(padding);
        }

        public double LarguraOriginal { get; set; }
        public double AlturaOriginal { get; set; }
        public double PaddingOriginal { get; set; }

        public double LarguraProcessada { get; set; }
        public double AlturaProcessada { get; set; }
        public double PaddingProcessado { get; set; }
    }
}
