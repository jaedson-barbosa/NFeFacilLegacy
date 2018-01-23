using NFeFacil.View;
using NFeFacil.ModeloXML.PartesDetalhes;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML
{
    public abstract class InformacoesBase
    {
        [XmlAttribute(AttributeName = "versao")]
        public string Versão = "3.10";

        [XmlAttribute(AttributeName = "Id")]
        public string Id
        {
            get => $"NFe{ChaveAcesso ?? AtualizarChave()}";
            set => ChaveAcesso = value.Remove(0, 3);
        }

        [XmlIgnore]
        public string ChaveAcesso { get; set; }

        [DescricaoPropriedade("Identificação")]
        [XmlElement(ElementName = "ide", Order = 0)]
        public Identificacao identificacao { get; set; }

        [DescricaoPropriedade("Emitente")]
        [XmlElement(ElementName = "emit", Order = 1)]
        public Emitente Emitente { get; set; }

        [DescricaoPropriedade("Cliente")]
        [XmlElement("dest", Order = 2)]
        public Destinatario destinatário { get; set; }

        [DescricaoPropriedade("Local de retirada")]
        [XmlElement("retirada", Order = 3)]
        public RetiradaOuEntrega Retirada { get; set; }

        [DescricaoPropriedade("Local de entrega")]
        [XmlElement("entrega", Order = 4)]
        public RetiradaOuEntrega Entrega { get; set; }

        public string AtualizarChave() => ChaveAcesso = new ChaveAcesso(this).CriarChaveAcesso();
    }
}