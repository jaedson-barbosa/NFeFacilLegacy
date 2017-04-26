using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao.Cliente;
using System;
using System.Threading.Tasks;

namespace BibliotecaCentral.Sincronizacao
{
    public sealed class GerenciadorCliente
    {
        private ILog Log { get; }
        public ItensBD.ResultadoSincronizacaoCliente Resultado { get; private set; }

        public GerenciadorCliente(ILog log)
        {
            Log = log;
        }

        public async Task Sincronizar(DadosSincronizaveis sincronizar, bool isBackground)
        {
            try
            {
                ItensSincronizados quantNotas = new ItensSincronizados(), quantDados = new ItensSincronizados();

                var config = await new ClienteConfiguracoes().ObterConfiguracoes();
                if (config.Notas && config.DadosBase && sincronizar == DadosSincronizaveis.Tudo)
                {
                    quantNotas = await new ClienteSincronizacaoNotas().Sincronizar();
                    quantDados = await new ClienteSincronizacaoDadosBase().Sincronizar();
                    Log.Escrever(TitulosComuns.Sucesso, "Foram sincronizados tanto notas fiscais quanto dados base para criação das notas fiscais.");
                }
                else if (config.Notas && sincronizar == DadosSincronizaveis.Tudo || sincronizar == DadosSincronizaveis.NotasFiscais)
                {
                    quantNotas = await new ClienteSincronizacaoNotas().Sincronizar();
                    Log.Escrever(TitulosComuns.Sucesso, "Apenas as notas fiscais puderam ser sincronizadas porque o servidor bloqueou a sincronização de dados base.");
                }
                else if (config.DadosBase && sincronizar == DadosSincronizaveis.Tudo || sincronizar == DadosSincronizaveis.DadosBase)
                {
                    quantDados = await new ClienteSincronizacaoDadosBase().Sincronizar();
                    Log.Escrever(TitulosComuns.Sucesso, "Apenas os dados base puderam ser sincronizados porque o servidor bloqueou a sincronização de dados base.");
                }
                else
                {
                    Log.Escrever(TitulosComuns.ErroSimples, "Nada pôde ser sincronizado porque o servidor bloqueou a sincronização do tipo de dado solicitado(s).");
                }

                using (var db = new AplicativoContext())
                {
                    db.Add(new ItensBD.ResultadoSincronizacaoCliente
                    {
                        PodeSincronizarDadoBase = config.DadosBase,
                        PodeSincronizarNota = config.Notas,
                        NumeroDadosEnviados = quantDados.Enviados,
                        NumeroDadosRecebidos = quantDados.Recebidos,
                        NumeroNotasEnviadas = quantNotas.Enviados,
                        NumeroNotasRecebidas = quantNotas.Recebidos,
                        MomentoSincronizacao = config.HoraRequisição,
                        SincronizacaoAutomatica = isBackground
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Log.Escrever(TitulosComuns.ErroCatastrófico, $"Erro: {e.Message}");
            }
        }
    }

    public enum DadosSincronizaveis
    {
        Tudo,
        DadosBase,
        NotasFiscais
    }
}
