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
        }

        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(CarregamentoCircular), new PropertyMetadata(2.0, OnPropertyChanged));
        public static readonly DependencyProperty SegmentoProperty = DependencyProperty.Register(nameof(Segmento), typeof(SolidColorBrush), typeof(CarregamentoCircular), new PropertyMetadata(default(Brush), OnPropertyChanged));
        public static readonly DependencyProperty CirculoCompletoProperty = DependencyProperty.Register(nameof(CirculoCompleto), typeof(SolidColorBrush), typeof(CarregamentoCircular), new PropertyMetadata(default(Brush), OnPropertyChanged));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(CarregamentoCircular), new PropertyMetadata(1.0, OnPropertyChanged));
        public static readonly DependencyProperty ActualValueProperty = DependencyProperty.Register(nameof(ActualValue), typeof(double), typeof(CarregamentoCircular), new PropertyMetadata(0.0, OnPropertyChanged));
        private static void OnPropertyChanged(object d, object e) => (d as CarregamentoCircular).Draw();

        public string Elemento
        {
            get => Texto.Text;
            set => Texto.Text = value;
        }

        public double Thickness
        {
            get => (double)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        public SolidColorBrush Segmento
        {
            get => (SolidColorBrush)GetValue(SegmentoProperty);
            set => SetValue(SegmentoProperty, value);
        }

        public SolidColorBrush CirculoCompleto
        {
            get => (SolidColorBrush)GetValue(CirculoCompletoProperty);
            set => SetValue(CirculoCompletoProperty, value);
        }

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public double ActualValue
        {
            get => (double)GetValue(ActualValueProperty);
            set => SetValue(ActualValueProperty, value);
        }

        private double Tamanho;
        protected override Size MeasureOverride(Size availableSize)
        {
            grd.Width = grd.Height = Tamanho = Math.Min(availableSize.Width, availableSize.Height);
            Draw();
            return availableSize;
        }

        private void Draw()
        {
            if (Tamanho > 0)
            {
                radialStrip.Data = ArcHelper.GetCircleSegment(CenterPoint, (Tamanho - Thickness) / 2, GetAngle());
                radialStrip.Stroke = Segmento;
                radialStrip.StrokeThickness = Thickness;
                complemento.Data = ArcHelper.GetCircleSegment(CenterPoint, (Tamanho - Thickness) / 2, 359.999);
                complemento.Stroke = CirculoCompleto;
                complemento.StrokeThickness = Thickness;
            }
        }

        private Point CenterPoint => new Point(Tamanho / 2, Tamanho / 2);
        private double GetAngle()
        {
            var angle = ActualValue / MaxValue * 360;
            if (angle >= 360) angle = 359.999;
            return angle;
        }
    }
}
