using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BaseGeral
{
    public sealed class BasicMainPage
    {
        public static BasicMainPage Current { get; } = new BasicMainPage();

        private BasicMainPage() { }

        public void Retornar()
        {
            var rootFrame = Window.Current.Content as IMainPage;
            rootFrame.Retornar();
        }

        public void Navegar<T>(object parametro = null) where T : Page
        {
            var rootFrame = Window.Current.Content as IMainPage;
            rootFrame.Navegar<T>(parametro);
        }
    }

    public interface IMainPage
    {
        void Retornar();
        void Navegar<T>(object parametro = null) where T : Page;
    }
}
