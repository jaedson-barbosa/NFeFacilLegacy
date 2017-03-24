using NFeFacil.Configuracoes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.ViewManagement;
using static NFeFacil.Configuracoes.ConfiguracoesSincronizacao;

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

        public ObservableCollection<ItensBD.ResultadoSincronizacaoCliente> ResultadosCliente
        {
            get
            {
                using (var db = new AplicativoContext())
                {
                    return db.ResultadosCliente.GerarObs();
                }
            }
        }

        public ObservableCollection<ItensBD.ResultadoSincronizacaoServidor> ResultadosServer
        {
            get
            {
                using (var db = new AplicativoContext())
                {
                    return db.ResultadosServudir.GerarObs();
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
