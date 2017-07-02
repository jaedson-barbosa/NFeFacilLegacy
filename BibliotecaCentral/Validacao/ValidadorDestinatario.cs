using BibliotecaCentral.ItensBD;
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

        public ValidadorDestinatario(ClienteDI dest)
        {
            Dest = dest.ToDestinatario();
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados(new Validadorendereco(Dest.Endereco)).ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Dest.Nome), "Não foi informado o nome do cliente"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Dest.Documento), "Não foi informado nenhum documento de identificação do cliente"));
        }
    }
}
