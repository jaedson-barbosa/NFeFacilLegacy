using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BaseGeral.Controles
{
    public sealed class RichEditBoxExtended : RichEditBox
    {
        bool lockChangeExecution;

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "TextProperty", typeof(string), typeof(RichEditBoxExtended),
                new PropertyMetadata(string.Empty, TextPropertyChanged));

        public RichEditBoxExtended()
        {
            TextChanged += RichEditBoxExtended_TextChanged;
        }

        void RichEditBoxExtended_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!lockChangeExecution)
            {
                lockChangeExecution = true;
                Document.GetText(TextGetOptions.None, out string text);
                text = text.Replace('\r', '\n');
                text = text.Remove(text.Length - 1, 1).Trim();
                Text = string.IsNullOrWhiteSpace(text) ? string.Empty : text;
                lockChangeExecution = false;
            }
        }

        static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var rtb = (RichEditBoxExtended)sender;
            if (!rtb.lockChangeExecution)
            {
                rtb.lockChangeExecution = true;
                rtb.Document.SetText(TextSetOptions.None, rtb.Text);
                rtb.lockChangeExecution = false;
            }
        }
    }
}
