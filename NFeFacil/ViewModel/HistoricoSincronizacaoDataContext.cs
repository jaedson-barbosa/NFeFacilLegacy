using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao;
using System.Collections.ObjectModel;
using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using BibliotecaCentral.Repositorio;
using System.Threading.Tasks;
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
            DefinirTudo();

            async void DefinirTudo()
            {
                await DefinirResultadosClienteAsync();
                await DefinirResultadosServidorAsync();
            }
        }

        private async Task DefinirResultadosClienteAsync()
        {
            using (var db = new ResultadosCliente())
            {
                ResultadosCliente = await Task.Run(() => db.Registro.GerarObs());
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResultadosCliente)));
            }
        }

        private async Task DefinirResultadosServidorAsync()
        {
            using (var db = new ResultadosServidor())
            {
                ResultadosServidor = await Task.Run(() => db.Registro.GerarObs());
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResultadosServidor)));
            }
        }
    }
}
