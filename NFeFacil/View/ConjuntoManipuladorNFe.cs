using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;

namespace NFeFacil.View
{
    internal sealed class ConjuntoManipuladorNFe
    {
        public NFe NotaSalva { get; set; }
        public Processo NotaEmitida { get; set; }
        public StatusNFe StatusAtual { get; set; }
        public TipoOperacao OperacaoRequirida { get; set; }
    }
}
