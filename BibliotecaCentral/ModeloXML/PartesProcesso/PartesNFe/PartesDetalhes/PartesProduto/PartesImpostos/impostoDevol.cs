using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    [XmlRoot("impostoDevol")]
    public class ImpostoDevol : Imposto
    {
        [XmlElement(Order = 0)]
        public string pDevol { get; set; }

        private IPIDevolvido ipi;
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

        public override bool IsValido => NaoNulos(pDevol, ipi);
        public class IPIDevolvido
        {
            [XmlElement(Order = 0)]
            public string vIPIDevol { get; set; }
        }
    }
}
