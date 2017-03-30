using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.View.Estilos.Auxiliares
{
    public sealed class PainelAdaptavel : Panel
    {
        private double _maxWidth;
        private double _maxHeight;
        private double colunas;
        private double linhas;

        public double TamanhoDesejado { get; set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            _maxWidth = Children.Max(x =>
            {
                x.Measure(TamanhoDesejado == 0 ? availableSize : new Size(TamanhoDesejado, availableSize.Height));
                return x.DesiredSize.Width;
            });
            _maxHeight = Children.Max(x => x.DesiredSize.Height);

            colunas = Math.Floor(availableSize.Width / _maxWidth);
            if (colunas > 5) colunas = 5;
            _maxWidth = Math.Ceiling(availableSize.Width / colunas);
            linhas = Math.Ceiling(Children.Count / colunas);
            return new Size(_maxWidth * colunas, _maxHeight * linhas);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var espacosX = from x in Enumerable.Range(0, (int)colunas)
                           select x * _maxWidth;
            var espacosY = from y in Enumerable.Range(0, (int)linhas)
                           select y * _maxHeight;
            var tamanho = new Size(_maxWidth, _maxHeight);
            for (int linha = 0, i = 0; linha < linhas; linha++)
            {
                for (int coluna = 0; coluna < colunas && i < Children.Count; coluna++, i++)
                {
                    var inicio = new Point(espacosX.ElementAt(coluna), espacosY.ElementAt(linha));
                    Children[i].Arrange(new Rect(inicio, tamanho));
                }
            }
            return finalSize;
        }
    }
}
