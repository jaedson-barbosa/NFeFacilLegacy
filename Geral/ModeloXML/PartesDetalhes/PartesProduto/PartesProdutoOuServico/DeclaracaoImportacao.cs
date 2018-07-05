using BaseGeral.View;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class DeclaracaoImportacao
    {
        [XmlElement("nDI", Order = 0), DescricaoPropriedade("Número do Documento de Importação")]
        public string NDI { get; set; }

        [XmlElement("dDI", Order = 1), DescricaoPropriedade("Data de Registro do documento")]
        public string DDI { get; set; }

        [XmlElement("xLocDesemb", Order = 2), DescricaoPropriedade("Local de desembaraço")]
        public string XLocDesemb { get; set; }

        [XmlElement("UFDesemb", Order = 3), DescricaoPropriedade("Sigla da UF onde ocorreu o Desembaraço Aduaneiro")]
        public string UFDesemb { get; set; }

        [XmlElement("dDesemb", Order = 4), DescricaoPropriedade("Data do Desembaraço Aduaneiro")]
        public string DDesemb { get; set; }

        [XmlElement("tpViaTransp", Order = 5), DescricaoPropriedade("Via de transporte internacional informada na Declaração de Importação")]
        public ushort TpViaTransp { get; set; }

        [XmlElement("vAFRMM", Order = 6), DescricaoPropriedade("Valor da AFRMM - Adicional ao Frete para Renovação da Marinha Mercante")]
        public string VAFRMM { get; set; }

        [XmlElement("tpIntermedio", Order = 7), DescricaoPropriedade("Forma de importação quanto a intermediação")]
        public ushort TpIntermedio { get; set; }

        [XmlElement(Order = 8), DescricaoPropriedade("")]
        public string CNPJ { get; set; }

        [XmlElement(Order = 9), DescricaoPropriedade("Sigla da UF do adquirente ou do encomendante")]
        public string UFTerceiro { get; set; }

        [XmlElement("cExportador", Order = 10), DescricaoPropriedade("Código do Exportador")]
        public string CExportador { get; set; }

        [XmlElement("adi", Order = 11), DescricaoPropriedade("")]
        public List<DIAdicao> Adi { get; set; } = new List<DIAdicao>();
    }
}
