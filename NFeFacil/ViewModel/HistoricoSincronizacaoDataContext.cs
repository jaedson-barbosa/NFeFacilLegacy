using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao;
using System.Collections.ObjectModel;
using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using System.ComponentModel;

namespace NFeFacil.ViewModel
{
    public sealed class HistoricoSincronizacaoDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCliente => Tipo == TipoAppSincronizacao.Cliente;
        public bool IsServidor => Tipo == TipoAppSincronizacao.Servidor;

        public ObservableCollection<ResultadoSincronizacaoCliente> ResultadosCliente { get; private set; }
        public ObservableCollection<ResultadoSincronizacaoServidor> ResultadosServidor { get; private set; }

        public HistoricoSincronizacaoDataContext()
        {
            using (var db = new AplicativoContext())
            {
                ResultadosCliente = db.ResultadosCliente.GerarObs();
                ResultadosServidor = db.ResultadosServidor.GerarObs();
            }
        }
    }
}
