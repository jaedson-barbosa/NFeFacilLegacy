using System;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.Controles
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class EntradaRestrita : UserControl
    {
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

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public string Pattern { get; set; }

        public bool MultiplePoints { get; set; }
        
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public void DefinirTexto(string value)
        {
            txtNumber.Text = value;
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(EntradaRestrita), new PropertyMetadata(null));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EntradaRestrita), new PropertyMetadata(null, NumeroMudou));

        static void NumeroMudou(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var input = (EntradaRestrita)sender;
            input.DefinirTexto((string)args.NewValue);
        }

        public EntradaRestrita()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = (TextBox)sender;
            if (input.FocusState != FocusState.Unfocused)
            {
                int pos = input.SelectionStart;
                input.Text = Regex.Replace(input.Text, Pattern, string.Empty).Replace(',', '.');
                input.SelectionStart = pos;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var input = (TextBox)sender;
            Text = input.Text;
        }

        private async void TextBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            var input = (TextBox)sender;
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                var texto = await dataPackageView.GetTextAsync();
                if (!MultiplePoints)
                {
                    var regex = new Regex(@"[.,]");
                    var ocorrencias = regex.Matches(texto);
                    texto = regex.Replace(texto, string.Empty, ocorrencias.Count - 1);
                }
                input.Text = Regex.Replace(texto, Pattern, string.Empty);
                e.Handled = true;
            }
        }
    }
}
