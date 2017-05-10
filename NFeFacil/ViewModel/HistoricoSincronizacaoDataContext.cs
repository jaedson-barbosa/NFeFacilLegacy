using BibliotecaCentral;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao;
using System.Collections.ObjectModel;
using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using BibliotecaCentral.Repositorio;

namespace NFeFacil.ViewModel
{
    public sealed class HistoricoSincronizacaoDataContext
    {
        public bool IsCliente => Tipo == TipoAppSincronizacao.Cliente;
        public bool IsServidor => Tipo == TipoAppSincronizacao.Servidor;

        public ObservableCollection<ResultadoSincronizacaoCliente> ResultadosCliente
        {
            get
            {
                using (var db = new ResultadosCliente())
                {
                    return db.Registro.GerarObs();
                }
            }
        }

        public ObservableCollection<ResultadoSincronizacaoServidor> ResultadosServer
        {
            get
            {
                using (var db = new ResultadosServidor())
                {
                    return db.Registro.GerarObs();
                }
            }
        }
    }
}
