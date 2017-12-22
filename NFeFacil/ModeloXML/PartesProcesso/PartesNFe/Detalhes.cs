using NFeFacil.AtributosVisualizacao;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe
{
    [XmlRoot("infNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class Detalhes
    {
        [XmlAttribute(AttributeName = "versao")]
        public string Versão = "3.10";

        [XmlAttribute(AttributeName = "Id")]
        public string Id
        {
            get
            {
                if (ChaveAcesso == null)
                {
                    AtualizarChave();
                }
                return $"NFe{ChaveAcesso}";
            }
            set
            {
                ChaveAcesso = value.Remove(0, 3);
            }
        }

        [XmlIgnore]
        public string ChaveAcesso { get; set; }

        [DescricaoPropriedade("Identificação")]
        [XmlElement(ElementName = "ide", Order = 0)]
        public Identificacao identificacao { get; set; }

        [DescricaoPropriedade("Emitente")]
        [XmlElement(ElementName = "emit", Order = 1)]
        public Emitente emitente { get; set; }

        [DescricaoPropriedade("Cliente")]
        [XmlElement("dest", Order = 2)]
        public Destinatario destinatário { get; set; }

        [DescricaoPropriedade("Local de retirada")]
        [XmlElement("retirada", Order = 3)]
        public RetiradaOuEntrega Retirada { get; set; }

        [DescricaoPropriedade("Local de entrega")]
        [XmlElement("entrega", Order = 4)]
        public RetiradaOuEntrega Entrega { get; set; }

        [DescricaoPropriedade("Produtos")]
        [XmlElement(ElementName = "det", Namespace = "http://www.portalfiscal.inf.br/nfe", Order = 5)]
        public List<DetalhesProdutos> produtos { get; set; }

        [DescricaoPropriedade("Total")]
        [XmlElement(Order = 6)]
        public Total total { get; set; }

        [DescricaoPropriedade("Transporte")]
        [XmlElement(Order = 7)]
        public Transporte transp { get; set; }

        [DescricaoPropriedade("Cobrança")]
        [XmlElement(Order = 8)]
        public Cobranca cobr { get; set; }

        [DescricaoPropriedade("Informações Adicionais")]
        [XmlElement(Order = 9)]
        public InformacoesAdicionais infAdic { get; set; }

        [DescricaoPropriedade("Exportação")]
        [XmlElement(Order = 10)]
        public Exportacao exporta { get; set; }

        [DescricaoPropriedade("Compra")]
        [XmlElement(Order = 11)]
        public Compra compra { get; set; }

        [DescricaoPropriedade("Cana de açúcar")]
        [XmlElement(Order = 12)]
        public RegistroAquisicaoCana cana { get; set; }

        public void AtualizarChave()
        {
            ChaveAcesso = new ChaveAcesso(this).CriarChaveAcesso();
        }
    }
}
