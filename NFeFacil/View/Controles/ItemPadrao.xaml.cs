using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    [ContentProperty(Name = "Icone")]
    public sealed partial class ItemPadrao : UserControl
    {
        public ItemPadrao()
        {
            InitializeComponent();
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ItemPadrao), new PropertyMetadata(null));

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public IconElement Icone { get; set; }

        private void Tocado(object sender, TappedRoutedEventArgs e)
        {
            if (Command?.CanExecute(null) ?? false)
            {
                Command.Execute(null);
            }
        }
    }
}
