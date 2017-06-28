using System.Globalization;
using System;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    public sealed partial class EntradaNumerica : UserControl
    {
        Regex regex = new Regex(@"[^\d,.]");
        string original = string.Empty;
        CultureInfo culturaPadrao = CultureInfo.InvariantCulture;

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
                    formatoProcessado = $"{Criar(partesFormato[0])}.{Criar(partesFormato[1])}";
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
            get => (double)GetValue(NumberProperty);
            set
            {
                var texto = DefinirTexto(value);
                SetValue(NumberProperty, double.Parse(texto, culturaPadrao));
            }
        }

        public string DefinirTexto(double value)
        {
            var texto = value.ToString(formatoProcessado, culturaPadrao);
            txtNumber.Text = texto;
            return texto;
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(string), typeof(EntradaNumerica), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(EntradaNumerica), new PropertyMetadata(null));
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(double), typeof(EntradaNumerica), new PropertyMetadata(null, NumeroMudou));

        static void NumeroMudou(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var input = (EntradaNumerica)sender;
            input.DefinirTexto((double)args.NewValue);
        }

        public EntradaNumerica()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = (TextBox)sender;
            if (input.FocusState != FocusState.Unfocused)
            {
                int pos = input.SelectionStart;
                input.Text = regex.Replace(input.Text, string.Empty).Replace(',', '.');
                input.SelectionStart = pos;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var input = (TextBox)sender;
            if (!string.IsNullOrEmpty(original))
            {
                input.Text = original;
            }
            else
            {
                input.Text = Number.ToString(culturaPadrao);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var input = (TextBox)sender;
            if (double.TryParse(input.Text, NumberStyles.AllowDecimalPoint, culturaPadrao, out double numero))
            {
                original = input.Text;
                Number = numero;
            }
            else
            {
                input.Focus(FocusState.Keyboard);
                //Agora deve ser exibido um aviso de que o formato está errado
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
}
