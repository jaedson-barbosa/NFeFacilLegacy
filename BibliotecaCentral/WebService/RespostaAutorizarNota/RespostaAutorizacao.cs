using BibliotecaCentral.IBGE;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService.RespostaAutorizarNota
{
    public struct RespostaAutorizacao
    {
        AutorizarNota.RetEnviNFe Recibo { get; }
        Estado UF => Estados.Buscar(Recibo.cUF);

        public RespostaAutorizacao(AutorizarNota.RetEnviNFe recibo)
        {
            Recibo = recibo;
        }

        public async Task<RetConsReciNFe> ObterRespostaAutorizacao(bool teste)
        {
            return await new GerenciadorGeral<ConsReciNFe, RetConsReciNFe>(UF, Operacoes.RespostaAutorizar, teste)
                .EnviarAsync(new ConsReciNFe(Recibo.tpAmb, Recibo.infRec.nRec));
        }
    }
}
