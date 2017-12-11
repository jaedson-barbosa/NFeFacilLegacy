using System;
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

        public DetalhePagina(string titulo)
        {
            Titulo = titulo;
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
    }
}
