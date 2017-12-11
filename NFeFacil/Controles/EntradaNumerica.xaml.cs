using System.Globalization;
using System;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.Controles
{
    public sealed partial class EntradaNumerica : UserControl
    {
        Regex regex = new Regex(@"[^\d,.]");
        CultureInfo culturaPadrao = CultureInfo.InvariantCulture;

        public event NumeroChangedEventHandler NumeroChanged;
        public event NumeroChangedEventHandler NumeroChanging;

        public InputScope InputScope
        {
            get => txtNumber.InputScope;
            set => txtNumber.InputScope = value;
        }

        public bool IsReadOnly
        {
            get => txtNumber.IsReadOnly;
            set => txtNumber.IsReadOnly = value;
        }

        string formatoProcessado;
        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set
            {
                var formatoOriginal = value;
                SetValue(FormatProperty, value);
                if (formatoOriginal.Contains(":"))
                {
                    var partesFormato = value.Split(':');
                    formatoProcessado = $"0.{Criar(partesFormato[1])}";/*Criar(partesFormato[0])*/
                }
                else
                {
                    formatoProcessado = Criar(formatoOriginal);
                }

                string Criar(string tamanho)
                {
                    return new string('0', int.Parse(tamanho));
                }
            }
        }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public double Number
        {
            get
            {
                var retorno = (double)Convert.ChangeType(GetValue(NumberProperty), typeof(double));
                return retorno;
            }
            set
            {
                var texto = DefinirTexto(value, value);
                var parseado = double.Parse(texto, culturaPadrao);
                SetValue(NumberProperty, parseado);
            }
        }

        public string DefinirTexto(IConvertible value0, IFormattable value1)
        {
            string texto;
            if (string.IsNullOrEmpty(formatoProcessado))
            {
                texto = value0.ToString(culturaPadrao);
            }
            else
            {
                texto = value1.ToString(formatoProcessado, culturaPadrao);
            }
            txtNumber.Text = texto;
            return texto;
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(string), typeof(EntradaNumerica), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(EntradaNumerica), new PropertyMetadata(null));
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(double), typeof(EntradaNumerica), new PropertyMetadata(null));

        public EntradaNumerica()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = (TextBox)sender;
            int pos = input.SelectionStart;
            input.Text = regex.Replace(input.Text, string.Empty).Replace(',', '.');
            input.SelectionStart = pos;

            if (NumeroChanging != null && !string.IsNullOrEmpty(input.Text))
            {
                var num = double.Parse(input.Text, culturaPadrao);
                NumeroChanging(this, new NumeroChangedEventArgs(num));
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var input = (TextBox)sender;
            input.SelectAll();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var input = (TextBox)sender;
            if (double.TryParse(input.Text, NumberStyles.Number, culturaPadrao, out double numero))
            {
                Number = numero;
                NumeroChanged?.Invoke(this, new NumeroChangedEventArgs(numero));
            }
            else
            {
                input.Focus(FocusState.Keyboard);
            }
        }

        private async void TextBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            var input = (TextBox)sender;
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                var texto = await dataPackageView.GetTextAsync();
                var regex = new Regex(@"[,.]");
                var ocorrencias = regex.Matches(texto);
                input.Text = regex.Replace(texto, string.Empty, ocorrencias.Count - 1);
                e.Handled = true;
            }
        }
    }

    public delegate void NumeroChangedEventHandler(EntradaNumerica sender, NumeroChangedEventArgs e);
}
