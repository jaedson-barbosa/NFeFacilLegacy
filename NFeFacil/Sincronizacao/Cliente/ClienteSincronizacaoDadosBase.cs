using static NFeFacil.Configuracoes.ConfiguracoesSincronizacao;
using System.Threading.Tasks;
using System.Linq;
using NFeFacil.Sincronizacao.Pacotes;

namespace NFeFacil.Sincronizacao.Cliente
{
    internal sealed class ClienteSincronizacaoDadosBase : ConexaoComServidor
    {
        public async Task<ItensSincronizados> Sincronizar()
        {
            var envio = ProcessamentoDadosBase.Obter();
            await Enviar(envio);
            var receb = await Receber();
            await ProcessamentoDadosBase.SalvarAsync(receb);
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
