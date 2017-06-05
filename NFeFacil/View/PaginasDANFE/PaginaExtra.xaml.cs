using NFeFacil.DANFE.Pacotes;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.PaginasDANFE
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class PaginaExtra : UserControl
    {
        double LarguraPagina => CentimeterToPixel(21);
        double AlturaPagina => CentimeterToPixel(29.7);

        public PaginaExtra(IEnumerable<DadosProduto> produtos, RichTextBlock infoAdicional, UIElementCollection paiPaginas, MotivoCriacaoPaginaExtra motivo)
        {
            this.InitializeComponent();
        }

        static double CentimeterToPixel(double Centimeter)
        {
            const double fator = 96 / 2.54;
            return Centimeter * fator;
        }
    }
}
