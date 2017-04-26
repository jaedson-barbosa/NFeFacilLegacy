using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
