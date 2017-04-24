using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class DeclaracaoImportacao
    {
        public string CNPJ { get; set; }

        /// <summary>
        /// Número do Documento de Importação.
        /// </summary>
        [XmlElement("nDI")]
        public ulong NDI { get; set; }

        /// <summary>
        /// Database.Principal de Registro do documento.
        /// </summary>
        [XmlElement("dDI")]
        public string DDI { get; set; }

        /// <summary>
        /// Local de desembaraço.
        /// </summary>
        [XmlElement("xLocDesemb")]
        public string XLocDesemb { get; set; }

        /// <summary>
        /// Sigla da UF onde ocorreu o Desembaraço Aduaneiro.
        /// </summary>
        public string UFDesemb { get; set; }

        /// <summary>
        /// Database.Principal do Desembaraço Aduaneiro.
        /// </summary>
        [XmlElement("dDesemb")]
        public string DDesemb { get; set; }

        /// <summary>
        /// Via de transporte internacional informada na Declaração de Importação.
        /// </summary>
        [XmlElement("tpViaTransp")]
        public ushort TpViaTransp { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor da AFRMM - Adicional ao Frete para Renovação da Marinha Mercante.
        /// A tag deve ser informada no caso da via de transporte marítima
        /// </summary>
        [XmlElement("vAFRMM")]
        public string VAFRMM { get; set; }

        /// <summary>
        /// Forma de importação quanto a intermediação.
        /// </summary>
        [XmlElement("tpIntermedio")]
        public ushort TpIntermedio { get; set; }

        /// <summary>
        /// (Opcional)
        /// Sigla da UF do adquirente ou do encomendante.
        /// Obrigatória a informação no caso de importação por conta e ordem ou por encomenda.Não aceita o valor "EX". 
        /// </summary>
        public string UFTerceiro { get; set; }

        /// <summary>
        /// Código do Exportador, usado nos sistemas internos de informação do emitente da NF-e.
        /// </summary>
        [XmlElement("cExportador")]
        public string CExportador { get; set; }

        [XmlElement("adi")]
        public List<DIAdicao> Adi { get; set; } = new List<DIAdicao>();
    }
}
