using System;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto
{
    public class BaseProdutoOuServico
    {
        [XmlIgnore]
        public Guid Id { get; set; }

        [XmlIgnore]
        public DateTime UltimaData { get; set; }

        /// <summary>
        /// Preencher com CFOP, caso se trate de itens não relacionados com mercadorias/produtos e que o contribuinte não possua codificação própria. Formato: ”CFOP9999”.
        /// </summary>
        [XmlElement(ElementName = "cProd")]
        public string CodigoProduto { get; set; }

        /// <summary>
        /// Não informar o conteúdo da TAG em caso de o Produto não possuir este código.
        /// </summary>
        [XmlElement(ElementName = "cEAN")]
        public string CodigoBarras { get; set; } = "";

        [XmlElement(ElementName = "xProd")]
        public string Descricao { get; set; }

        /// <summary>
        /// Obrigatória informação do NCM completo (8 dígitos).
        /// Em caso de item de serviço ou item que não tenham Produto (ex. transferência de crédito), informar o valor 00 (dois zeros).
        /// </summary>
        public string NCM { get; set; }

        /// <summary>
        /// (Opcional)
        /// Preencher de acordo com o código EX da TIPI. Em caso de serviço, não incluir a TAG.
        /// </summary>
        public string EXTIPI { get; set; }

        /// <summary>
        /// Código Fiscal de Operações e Prestações.
        /// </summary>
        public string CFOP { get; set; }

        /// <summary>
        /// Informar a unidade de comercialização do Produto.
        /// </summary>
        [XmlElement(ElementName = "uCom")]
        public string UnidadeComercializacao { get; set; }

        /// <summary>
        /// Informar o valor unitário de comercialização do Produto.
        /// </summary>
        [XmlElement(ElementName = "vUnCom")]
        public double ValorUnitario { get; set; }

        /// <summary>
        /// GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras.
        /// Não informar o conteúdo da TAG em caso de o Produto não possuir este código.
        /// </summary>
        [XmlElement(ElementName = "cEANTrib")]
        public string CodigoBarrasTributo { get; set; } = "";

        /// <summary>
        /// Unidade Tributável.
        /// </summary>
        [XmlElement(ElementName = "uTrib")]
        public string UnidadeTributacao { get; set; }

        /// <summary>
        /// Informar o valor unitário de tributação do Produto.
        /// </summary>
        [XmlElement(ElementName = "vUnTrib")]
        public double ValorUnitarioTributo { get; set; }
    }
}
