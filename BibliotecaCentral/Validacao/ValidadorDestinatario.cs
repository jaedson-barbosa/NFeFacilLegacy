using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace BibliotecaCentral.Validacao
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
            return new ValidarDados(new Validadorendereco(Dest.endereco)).ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Dest.nome), "Não foi informado o nome do cliente"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Dest.obterDocumento), "Não foi informado nenhum documento de identificação do cliente"));
        }
    }
}
