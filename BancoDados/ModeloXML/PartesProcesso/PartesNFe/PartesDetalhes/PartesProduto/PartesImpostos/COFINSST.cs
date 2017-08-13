using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo COFINS Substituição Tributária.
    /// Só deve ser informado se o Produto for sujeito a COFINS por ST (CST = 05).
    /// </summary>
    public class COFINSST : Imposto
    {
        /// <summary>
        /// Valor da Base de Cálculo da COFINS.
        /// </summary>
        [XmlElement(Order = 0)]
        public string vBC { get; set; }

        /// <summary>
        /// Alíquota da COFINS (em percentual).
        /// </summary>
        [XmlElement(Order = 1)]
        public string pCOFINS { get; set; }

        /// <summary>
        /// Quantidade Vendida.
        /// </summary>
        [XmlElement(Order = 2)]
        public string qBCProd { get; set; }

        /// <summary>
        /// Alíquota da COFINS (em reais).
        /// </summary>
        [XmlElement(Order = 3)]
        public string vAliqProd { get; set; }

        /// <summary>
        /// Valor da COFINS.
        /// </summary>
        [XmlElement(Order = 4)]
        public string vCOFINS { get; set; }

        public override bool IsValido
        {
            get
            {
                return NaoNulos(vBC, pCOFINS, qBCProd, vAliqProd, vCOFINS);
            }
        }
    }
}
