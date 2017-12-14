using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class ProcessoReferenciado
    {
        /// <summary>
        /// Identificador do processo ou ato concessório.
        /// </summary>
        [XmlElement("nProc", Order = 0), DescricaoPropriedade("Identificador do processo ou ato concessório")]
        public string NProc { get; set; }

        /// <summary>
        /// Indicador da origem do processo.
        /// </summary>
        [XmlElement("indProc", Order = 1), DescricaoPropriedade("Indicador da origem do processo")]
        public int IndProc { get; set; }

        public override string ToString()
        {
            string origem;
            switch (IndProc)
            {
                case 0:
                    origem = "SEFAZ";
                    break;
                case 1:
                    origem = "Justiça Federal";
                    break;
                case 2:
                    origem = "Justiça Estadual";
                    break;
                case 3:
                    origem = "Secex/RFB";
                    break;
                case 9:
                    origem = "Outros";
                    break;
                default:
                    origem = "Desconhecida";
                    break;
            }
            return $"Identificados: {NProc}; Origem: {origem}";
        }
    }
}
