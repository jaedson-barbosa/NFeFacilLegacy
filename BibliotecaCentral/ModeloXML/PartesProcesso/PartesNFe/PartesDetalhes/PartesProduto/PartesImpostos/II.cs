using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class II : Imposto
    {
        /// <summary>
        /// Valor BC do Imposto de Importação.
        /// </summary>
        [XmlElement(Order = 0)]
        public string vBC { get; set; }

        /// <summary>
        /// Valor despesas aduaneiras.
        /// </summary>
        [XmlElement(Order = 1)]
        public string vDespAdu { get; set; }

        /// <summary>
        /// Valor Imposto de Importação.
        /// </summary>
        [XmlElement(Order = 2)]
        public string vII { get; set; }

        /// <summary>
        /// Valor Imposto sobre Operações Financeiras.
        /// </summary>
        [XmlElement(Order = 3)]
        public string vIOF { get; set; }

        public override bool IsValido => NaoNulos(vBC, vDespAdu, vII, vIOF);
    }
}
