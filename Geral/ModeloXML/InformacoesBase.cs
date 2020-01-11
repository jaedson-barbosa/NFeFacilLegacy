using BaseGeral.View;
using BaseGeral.ModeloXML.PartesDetalhes;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML
{
    public abstract class InformacoesBase
    {
        [XmlAttribute(AttributeName = "versao")]
        public string Versão = "4.00";

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
        public Destinatario destinatario { get; set; }

        public string AtualizarChave() => ChaveAcesso = new ChaveAcesso(this).CriarChaveAcesso();
    }
}