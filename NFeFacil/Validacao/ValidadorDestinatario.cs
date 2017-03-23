using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace NFeFacil.Validacao
{
    public sealed class ValidadorDestinatario : IValidavel
    {
        private Destinatario Dest;

        public ValidadorDestinatario(Destinatario dest)
        {
            Dest = dest;
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados(new ValidadorEndereco(Dest.endereço)).ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Dest.nome), "Não foi informado o nome do cliente"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Dest.obterDocumento), "Não foi informado nenhum documento de identificação do cliente"));
        }
    }
}
