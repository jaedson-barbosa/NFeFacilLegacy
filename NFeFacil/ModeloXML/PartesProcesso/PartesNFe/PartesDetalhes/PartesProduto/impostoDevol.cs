using NFeFacil.AtributosVisualizacao;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto
{
    [XmlRoot("impostoDevol")]
    public class ImpostoDevol
    {
        [XmlElement(Order = 0)]
        public string pDevol { get; set; }

        private IPIDevolvido ipi;

        [XmlElement(Order = 1), DescricaoPropriedade("IPI Devolvido")]
        public IPIDevolvido IPI
        {
            get
            {
                if (ipi == null) ipi = new IPIDevolvido();
                return ipi;
            }
            set
            {
                ipi = value;
            }
        }

        public class IPIDevolvido
        {
            [XmlElement(Order = 0), DescricaoPropriedade("Valor do IPI devolvido")]
            public string vIPIDevol { get; set; }
        }
    }
}
