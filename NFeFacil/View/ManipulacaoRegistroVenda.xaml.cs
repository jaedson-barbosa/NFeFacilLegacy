using NFeFacil.ItensBD;
using NFeFacil.ViewModel;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoRegistroVenda : Page
    {
        public ManipulacaoRegistroVenda()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RegistroVendaDataContext contexto;
            if (e.Parameter is GrupoViewBanco<RegistroVenda> grupo)
            {
                contexto = new RegistroVendaDataContext(grupo.ItemBanco);
            }
            else
            {
                contexto = new RegistroVendaDataContext();
            }
            DataContext = contexto;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ((IDisposable)DataContext).Dispose();
        }
    }
}
