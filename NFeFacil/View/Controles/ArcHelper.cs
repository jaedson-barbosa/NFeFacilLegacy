using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    public static class ArcHelper
    {
        public static Geometry GetCircleSegment(Point centerPoint, double radius, double angle)
        {
            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure
            {
                StartPoint = new Point(centerPoint.X, centerPoint.Y - radius),
                IsClosed = false
            };
            pathFigure.Segments.Add(new ArcSegment
            {
                IsLargeArc = angle > 180.0,
                Point = ScaleUnitCirclePoint(centerPoint, angle, radius),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise
            });
            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private static Point ScaleUnitCirclePoint(Point origin, double angle, double radius)
        {
            var retorno = new Point(origin.X + Math.Sin(GrauParaRadiano(angle)) * radius, origin.Y - Math.Cos(GrauParaRadiano(angle)) * radius);
            return retorno;
        }

        private static double GrauParaRadiano(double grau) => Math.PI * grau / 180;
    }
}
