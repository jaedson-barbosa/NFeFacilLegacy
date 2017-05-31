using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System.Collections.Generic;

namespace NFeFacil.DANFE.Pacotes
{
    public sealed class DadosAdicionais
    {
        public string Dados { get; set; } = "";
        public string Fisco { get; set; } = "";
        public List<Duplicata> Duplicatas { get; set; }
    }
}
