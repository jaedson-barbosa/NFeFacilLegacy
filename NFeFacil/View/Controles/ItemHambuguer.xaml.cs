using System;
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

        public ItemHambuguer(string glyph, string descricao) : this()
        {
            Icone = new FontIcon()
            {
                Glyph = glyph
            };
            Descricao = descricao;
        }

        public ItemHambuguer(Symbol simbolo, string descricao) : this()
        {
            Icone = new SymbolIcon(simbolo);
            Descricao = descricao;
        }

        public ItemHambuguer(Uri uri, string descricao) : this()
        {
            Icone = new BitmapIcon()
            {
                UriSource = uri
            };
            Descricao = descricao;
        }

        public IconElement Icone { get; set; }
        public string Descricao { get; set; }
    }
}
