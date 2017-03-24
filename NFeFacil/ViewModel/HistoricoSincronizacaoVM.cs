using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.ViewManagement;

namespace NFeFacil.ViewModel
{
    public sealed class HistoricoSincronizacaoVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ApplicationView View { get; }

        public HistoricoSincronizacaoVM()
        {
            View = ApplicationView.GetForCurrentView();
            View.VisibleBoundsChanged += (x, y) => PropertyChanged(this, new PropertyChangedEventArgs("Vertical"));
        }

        public bool IsCliente
        {
            get { return Tipo == TipoAppSincronizacao.Cliente; }
        }

        public bool IsServidor
        {
            get { return Tipo == TipoAppSincronizacao.Servidor; }
        }

        public ObservableCollection<ResultClienteDataItem> ResultadosCliente
        {
            get
            {
                using (var tabela = new TabelaResultCliente())
                {
                    return tabela.RegistroCompleto().GerarObs();
                }
            }
        }

        public ObservableCollection<ResultServerDataItem> ResultadosServer
        {
            get
            {
                using (var tabela = new TabelaResultServer())
                {
                    return tabela.RegistroCompleto().GerarObs();
                }
            }
        }

        public bool Vertical
        {
            get
            {
                return View.Orientation == ApplicationViewOrientation.Portrait;
            }
        }
    }
}
