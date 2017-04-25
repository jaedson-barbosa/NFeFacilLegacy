using static BibliotecaCentral.Configuracoes.ConfiguracoesSincronizacao;
using System.Threading.Tasks;
using System.Linq;
using BibliotecaCentral.Sincronizacao.Pacotes;

namespace BibliotecaCentral.Sincronizacao.Cliente
{
    internal sealed class ClienteSincronizacaoDadosBase : ConexaoComServidor
    {
        public async Task<ItensSincronizados> Sincronizar()
        {
            using (var proc = new ProcessamentoDadosBase())
            {
                var envio = proc.Obter();
                await Enviar(envio);
                var receb = await Receber();
                await proc.SalvarAsync(receb);
                return new ItensSincronizados(CalcularTotal(envio), CalcularTotal(receb));
            }
        }

        private int CalcularTotal(DadosBase dados)
        {
            return dados.Clientes.Count() + dados.Emitentes.Count() + dados.Motoristas.Count() + dados.Produtos.Count();
        }

        private async Task Enviar(DadosBase pacote)
        {
            await SendRequest<string>($"Dados", Método.POST, SenhaPermanente, pacote);
        }

        private async Task<DadosBase> Receber()
        {
            return await SendRequest<DadosBase>($"Dados", Método.GET, SenhaPermanente, null);
        }
    }
}
