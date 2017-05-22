using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    [Windows.UI.Xaml.Markup.ContentProperty(Name = nameof(Icone))]
    public sealed partial class ItemHambuguer : UserControl
    {
        public ItemHambuguer()
        {
            this.InitializeComponent();
        }

        public IconElement Icone { get; set; }
        public string Descricao { get; set; }
    }
}
