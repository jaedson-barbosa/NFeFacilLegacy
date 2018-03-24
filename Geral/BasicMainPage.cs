using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace BaseGeral
{
    public sealed class BasicMainPage
    {
        public static BasicMainPage Current { get; } = new BasicMainPage();

        private BasicMainPage() { }

        public void Retornar()
        {

        }

        public void Navegar<T>(object parametro = null) where T : Page
        {

        }
    }
}
