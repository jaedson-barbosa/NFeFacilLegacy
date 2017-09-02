using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using System;

namespace NFeFacil
{
    internal sealed class ConjuntoManipuladorNFe
    {
        public NFe NotaSalva { get; set; }
        public Processo NotaEmitida { get; set; }
        public StatusNFe StatusAtual { get; set; }
        public bool Exportada { get; set; }
        public bool Impressa { get; set; }
        public Action<string> OnNotaSalva { get; set; }
    }
}
