using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;

namespace NFeFacil
{
    internal sealed class ConjuntoManipuladorNFe
    {
        public NFe NotaSalva { get; set; }
        public Processo NotaEmitida { get; set; }
        public StatusNFe StatusAtual { get; set; }
        public bool Exportada { get; set; }
        public bool Impressa { get; set; }
        public TipoOperacao OperacaoRequirida { get; set; }
    }
}
