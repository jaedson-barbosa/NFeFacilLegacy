using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using System.Threading.Tasks;
using System.Linq;
using BibliotecaCentral.Sincronizacao.Pacotes;

namespace BibliotecaCentral.Sincronizacao.Cliente
{
    internal sealed class ClienteSincronizacaoDadosBase : ConexaoComServidor
    {
        public async Task<ItensSincronizados> Sincronizar()
        {
            var envio = ProcessamentoDadosBase.Obter();
            await Enviar(envio);
            var receb = await Receber();
            ProcessamentoDadosBase.Salvar(receb);
            return new ItensSincronizados(CalcularTotal(envio), CalcularTotal(receb));
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
