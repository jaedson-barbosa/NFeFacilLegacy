using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    public sealed partial class CarregamentoCircular : UserControl
    {
        public CarregamentoCircular()
        {
            InitializeComponent();
            SetControlSize();
            Draw();
        }

        public string Elemento
        {
            get
            {
                return Texto.Text;
            }
            set
            {
                Texto.Text = value;
            }
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(CarregamentoCircular), new PropertyMetadata(50.0, OnSizePropertyChanged));
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(CarregamentoCircular), new PropertyMetadata(2.0, OnSizePropertyChanged));
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(SolidColorBrush), typeof(CarregamentoCircular), new PropertyMetadata(default(Brush)));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(CarregamentoCircular), new PropertyMetadata(1.0, OnPercentValuePropertyChanged));
        public static readonly DependencyProperty ActualValueProperty = DependencyProperty.Register(nameof(ActualValue), typeof(double), typeof(CarregamentoCircular), new PropertyMetadata(0.0, OnPercentValuePropertyChanged));

        public double Radius
        {
            get
            {
                return (double)GetValue(RadiusProperty);
            }
            set
            {
                SetValue(RadiusProperty, value);
            }
        }

        public double Thickness
        {
            get
            {
                return (double)GetValue(ThicknessProperty);
            }
            set
            {
                SetValue(ThicknessProperty, value);
            }
        }

        public SolidColorBrush Fill
        {
            get
            {
                return (SolidColorBrush)GetValue(FillProperty);
            }
            set
            {
                SetValue(FillProperty, value);
            }
        }

        public double MaxValue
        {
            get
            {
                return (double)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public double ActualValue
        {
            get
            {
                return (double)GetValue(ActualValueProperty);
            }
            set
            {
                SetValue(ActualValueProperty, value);
            }
        }

        private static void OnSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CarregamentoCircular;
            control.SetControlSize();
            control.Draw();
        }

        private static void OnPercentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CarregamentoCircular;
            control.SetControlSize();
            control.Draw();
        }

        private void Draw()
        {
            radialStrip.Data = ArcHelper.GetCircleSegment(GetCenterPoint(), Radius, GetAngle());
            radialStrip.Stroke = Fill;
            radialStrip.StrokeThickness = Thickness;
        }

        private void SetControlSize()
        {
            Width = Height = Radius * 2 + Thickness;
        }

        private Point GetCenterPoint() => new Point(Radius + (Thickness / 2), Radius + (Thickness / 2));

        private double GetAngle()
        {
            var angle = ActualValue / MaxValue * 360;
            if (angle >= 360) angle = 359.999;
            return angle;
        }
    }

    public static class ArcHelper
    {
        private const double RADIANS = Math.PI / 180;

        public static Geometry GetCircleSegment(Point centerPoint, double radius, double angle)
        {
            var pathGeometry = new PathGeometry();

            var circleStart = new Point(centerPoint.X, centerPoint.Y - radius);

            var arcSegment = new ArcSegment
            {
                IsLargeArc = angle > 180.0,
                Point = ScaleUnitCirclePoint(centerPoint, angle, radius),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise
            };

            var pathFigure = new PathFigure
            {
                StartPoint = circleStart,
                IsClosed = false
            };

            pathFigure.Segments.Add(arcSegment);
            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private static Point ScaleUnitCirclePoint(Point origin, double angle, double radius)
        {
            return new Point(origin.X + Math.Sin(RADIANS * angle) * radius, origin.Y - Math.Cos(RADIANS * angle) * radius);
        }
    }
}
