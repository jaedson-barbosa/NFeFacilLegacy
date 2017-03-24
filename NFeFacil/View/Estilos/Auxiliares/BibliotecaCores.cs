using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace NFeFacil.View.Estilos.Auxiliares
{
    public class BibliotecaCores
    {
        public Color Cor0
        {
            get { return ObterCor(UIColorType.Accent, UIColorType.Accent); }
        }

        public Color Cor1
        {
            get { return ObterCor(UIColorType.AccentDark1, UIColorType.AccentLight1); }
        }

        public Color Cor2
        {
            get { return ObterCor(UIColorType.AccentDark2, UIColorType.AccentLight2); }
        }

        public Color Cor3
        {
            get { return ObterCor(UIColorType.AccentDark3, UIColorType.AccentLight3); }
        }

        public ApplicationTheme Tema { get; } = Application.Current.RequestedTheme;

        private Color ObterCor(UIColorType casoEscuro, UIColorType casoClaro)
        {
            var cor = Tema == ApplicationTheme.Dark ? casoEscuro : casoClaro;
            return new UISettings().GetColorValue(cor);
        }
    }
}
