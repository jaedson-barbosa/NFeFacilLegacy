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

        public void AtualizarChave()
        {
            ChaveAcesso = new ChaveAcesso(this).CriarChaveAcesso();
        }

        [XmlElement(ElementName = "ide", Order = 0)]
        public Identificacao identificacao { get; set; }

        [XmlElement(ElementName = "emit", Order = 1)]
        public Emitente emitente { get; set; }

        [XmlElement("dest", Order = 2)]
        public Destinatario destinatário { get; set; }

        /// <summary>
        /// (Opcional)
        /// Identificação do Local de retirada.
        /// </summary>
        [XmlElement("retirada", Order = 3)]
        public RetiradaOuEntrega Retirada { get; set; }

        /// <summary>
        /// (Opcional)
        /// Identificação do Local de entrega.
        /// </summary>
        [XmlElement("entrega", Order = 4)]
        public RetiradaOuEntrega Entrega { get; set; }

        [XmlElement(ElementName = "det", Namespace = "http://www.portalfiscal.inf.br/nfe", Order = 5)]
        public List<DetalhesProdutos> produtos { get; set; }

        [XmlElement(Order = 6)]
        public Total total { get; set; }

        [XmlElement(Order = 7)]
        public Transporte transp { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Cobrança.
        /// </summary>
        [XmlElement(Order = 8)]
        public Cobranca cobr { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo de Informações Adicionais.
        /// </summary>
        [XmlElement(Order = 9)]
        public InformacoesAdicionais infAdic { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Exportação.
        /// </summary>
        [XmlElement(Order = 10)]
        public Exportacao exporta { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Compra.
        /// </summary>
        [XmlElement(Order = 11)]
        public Compra compra { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Cana.
        /// </summary>
        [XmlElement(Order = 12)]
        public RegistroAquisicaoCana cana { get; set; }
    }
}
