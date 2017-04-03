using NFeFacil.Configuracoes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.ViewManagement;
using static NFeFacil.Configuracoes.ConfiguracoesSincronizacao;

namespace NFeFacil.ViewModel
{
    public sealed class HistoricoSincronizacaoDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ApplicationView View { get; }

        public HistoricoSincronizacaoDataContext()
        {
            View = ApplicationView.GetForCurrentView();
            View.VisibleBoundsChanged += (x, y) => PropertyChanged(this, new PropertyChangedEventArgs("Vertical"));
        }

        public bool IsCliente => Tipo == TipoAppSincronizacao.Cliente;
        public bool IsServidor => Tipo == TipoAppSincronizacao.Servidor;
        public bool Vertical => View.Orientation == ApplicationViewOrientation.Portrait;

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
                    return db.ResultadosServidor.GerarObs();
                }
            }
        }

    }
}
