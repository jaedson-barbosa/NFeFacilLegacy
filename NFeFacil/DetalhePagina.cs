using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NFeFacil
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    sealed class DetalhePagina : Attribute
    {
        public string Titulo { get; }
        public Uri SimboloUri { get; set; }
        public string SimboloGlyph { get; set; }
        public Symbol SimboloSymbol { get; set; }

        public DetalhePagina(string titulo) => Titulo = titulo;
        public DetalhePagina(string glyph, string texto) : this(texto) => SimboloGlyph = glyph;
        public DetalhePagina(Symbol símbolo, string texto) : this(texto) => SimboloSymbol = símbolo;

        public DetalhePagina(SimbolosEspeciais simbolo, string texto) : this(texto)
        {
            switch (simbolo)
            {
                case SimbolosEspeciais.Arma:
                    var usarDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
                    var caminho = usarDark ? "ms-appx:///Assets/ArmaDark.png" : "ms-appx:///Assets/Arma.png";
                    SimboloUri = new Uri(caminho);
                    break;
                default:
                    break;
            }
        }

        public IconElement ObterIcone()
        {
            if (SimboloUri != null)
            {
                return new BitmapIcon { UriSource = SimboloUri };
            }
            else if (SimboloGlyph != null)
            {
                return new FontIcon { Glyph = SimboloGlyph };
            }
            else
            {
                return new SymbolIcon(SimboloSymbol);
            }
        }

        public enum SimbolosEspeciais { Arma }
    }
}
