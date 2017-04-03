using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;

namespace NFeFacil
{
    internal sealed class NotaComDados
    {
        public NFe nota;
        public Processo proc;
        public NFeDI dados;
        public TipoOperacao tipoRequisitado;
    }
}