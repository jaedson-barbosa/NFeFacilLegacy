using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace BaseGeral.View
{
    public class BibliotecaCores
    {
        public Color Cor0 { get; } = ObterCor(UIColorType.Accent, UIColorType.Accent);
        public Color Cor1 { get; } = ObterCor(UIColorType.AccentDark1, UIColorType.AccentLight1);
        public Color Cor2 { get; } = ObterCor(UIColorType.AccentDark2, UIColorType.AccentLight2);
        public Color Cor3 { get; } = ObterCor(UIColorType.AccentDark3, UIColorType.AccentLight3);

        static Color ObterCor(UIColorType casoEscuro, UIColorType casoClaro)
        {
            var tema = Application.Current.RequestedTheme;
            var cor = tema == ApplicationTheme.Dark ? casoEscuro : casoClaro;
            return new UISettings().GetColorValue(cor);
        }
    }
}
