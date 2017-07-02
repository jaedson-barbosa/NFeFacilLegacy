using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class Combustivel
    {
        /// <summary>
        /// Código de Produto da ANP.
        /// Utilizar a codificação de produtos do SIMP.
        /// </summary>
        [XmlElement("cProdANP", Order = 0)]
        public int CProdANP { get; set; }

        /// <summary>
        /// (Opcional)
        /// Percentual de Gás Natural para o Produto GLP.
        /// </summary>
        [XmlElement("pMixGN", Order = 1)]
        public string PMixGN { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar apenas quando a UF utilizar o CODIF
        /// (Sistema de Controle do Diferimento do Imposto nas Operações com AEAC - Álcool Etílico Anidro Combustível).
        /// </summary>
        [XmlElement(Order = 2)]
        public string CODIF { get; set; }

        /// <summary>
        /// (Opcional)
        /// Quantidade de combustível faturada à temperatura ambiente.
        /// </summary>
        [XmlElement("qTemp", Order = 3)]
        public string QTemp { get; set; }

        /// <summary>
        /// Informar a UF de consumo. Informar "EX" para Exterior.
        /// </summary>
        [XmlElement(Order = 4)]
        public string UFCons { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo de informações da CIDE.
        /// </summary>
        [XmlElement(Order = 5)]
        public CIDE CIDE { get; set; }
    }
}
