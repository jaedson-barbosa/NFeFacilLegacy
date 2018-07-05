using System;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BaseGeral.Controles
{
    [Windows.UI.Xaml.Markup.ContentProperty(Name = nameof(Icone))]
    public sealed partial class ItemHambuguer : UserControl
    {
        public ItemHambuguer(string glyph, string descricao)
        {
            Icone = new FontIcon()
            {
                Glyph = glyph
            };
            Descricao = descricao;
            InitializeComponent();
        }

        public ItemHambuguer(Symbol simbolo, string descricao)
        {
            Icone = new SymbolIcon(simbolo);
            Descricao = descricao;
            InitializeComponent();

        }

        public ItemHambuguer(Uri uri, string descricao)
        {
            Icone = new BitmapIcon()
            {
                UriSource = uri
            };
            Descricao = descricao;
            InitializeComponent();
        }

        public IconElement Icone { get; set; }
        public string Descricao { get; set; }
    }
}
