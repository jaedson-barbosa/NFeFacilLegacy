using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BaseGeral
{
    public static class LimitadoresTextuais
    {
        public static void PermitirNumeros(object sender, TextChangedEventArgs e) => AplicarRegex(sender, @"[^\d]");

        private static void AplicarRegex(object sender, string pattern)
        {
            var input = (TextBox)sender;
            if (input.FocusState != FocusState.Unfocused)
            {
                int pos = input.SelectionStart;
                var texto = Regex.Replace(input.Text, pattern, string.Empty).Replace(',', '.');
                if (texto.IndexOf('.') != texto.LastIndexOf('.'))
                {
                    var regex = new Regex(@"[.,]");
                    var ocorrencias = regex.Matches(texto);
                    texto = regex.Replace(texto, string.Empty, ocorrencias.Count - 1);
                }
                input.Text = texto;
                input.SelectionStart = pos;
            }
        }
    }
}
