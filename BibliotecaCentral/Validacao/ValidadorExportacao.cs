using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.Log;

namespace BibliotecaCentral.Validacao
{
    public sealed class ValidadorExportacao : IValidavel
    {
        private Exportacao Exportacao;

        public ValidadorExportacao(Exportacao exportacao)
        {
            Exportacao = exportacao;
        }

        public bool Validar(ILog log)
        {
            if (Exportacao == null) return false;
            return new ValidarDados().ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Exportacao.UFSaidaPais), "Não foi definida uma UF de saída."),
                new ConjuntoAnalise(string.IsNullOrEmpty(Exportacao.XLocExporta), "Não foi definido o local de exportação"));
        }
    }
}
