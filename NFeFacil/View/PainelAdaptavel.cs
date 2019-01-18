using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.View
{
    public class PainelAdaptavel : Panel
    {
        private double _maxWidth;
        private double _maxHeight;
        private double colunas;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Children.Count > 1)
            {
                _maxWidth = Children.Max(x =>
                {
                    x.Measure(availableSize);
                    return x.DesiredSize.Width;
                });
                _maxHeight = Children.Max(x => x.DesiredSize.Height);

                colunas = Math.Floor(availableSize.Width / _maxWidth);
                _maxWidth = Math.Floor(availableSize.Width / colunas);
                return new Size(_maxWidth * colunas, _maxHeight * Math.Ceiling(Children.Count / colunas));
            }
            else if (Children.Count == 1)
            {
                var filho = Children[0];
                filho.Measure(availableSize);
                _maxWidth =  filho.DesiredSize.Width;
                _maxHeight = filho.DesiredSize.Height;

                colunas = Math.Floor(availableSize.Width / _maxWidth);
                _maxWidth = Math.Floor(availableSize.Width / colunas);
                return new Size(_maxWidth * colunas, _maxHeight * Math.Ceiling(Children.Count / colunas));
            }
            else
            {
                return new Size(100, 100);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count > 0)
            {
                var tamanho = new Size(_maxWidth, _maxHeight);
                double maxX = (colunas - 1) * _maxWidth;

                double x = 0, y = 0;
                for (int k = 0; k < Children.Count; k++)
                {
                    var inicio = new Point(x, y);
                    Children[k].Arrange(new Rect(inicio, tamanho));

                    if (x >= maxX)
                    {
                        x = 0;
                        y += _maxHeight;
                    }
                    else
                    {
                        x += _maxWidth;
                    }
                }
            }

            return finalSize;
        }
    }
}
