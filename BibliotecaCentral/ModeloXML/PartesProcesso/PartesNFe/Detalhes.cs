using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe
{
    [XmlRoot("infNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class Detalhes
    {
        [XmlAttribute(AttributeName = "versao")]
        public string Versão = "3.10";

        private string chaveAcesso;
        [XmlAttribute(AttributeName = "Id")]
        public string Id
        {
            get
            {
                return $"NFe{chaveAcesso ?? (chaveAcesso = new ChaveAcesso(this).CriarChaveAcesso())}";
            }
            set
            {
                chaveAcesso = value.Substring(value.IndexOf('e') + 1);
            }
        }

        [XmlElement(ElementName = "ide")]
        public Identificacao identificação { get; set; }

        [XmlElement(ElementName = "emit")]
        public Emitente emitente { get; set; }

        [XmlElement("dest")]
        public Destinatario destinatário { get; set; }

        /// <summary>
        /// (Opcional)
        /// Identificação do Local de retirada.
        /// </summary>
        public RetiradaOuEntrega retirada { get; set; }

        /// <summary>
        /// (Opcional)
        /// Identificação do Local de entrega.
        /// </summary>
        public RetiradaOuEntrega entrega { get; set; }

        [XmlElement(ElementName = "det", Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public List<DetalhesProdutos> produtos { get; set; }

        public Total total { get; set; }

        public Transporte transp { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Cobrança.
        /// </summary>
        public Cobranca cobr { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo de Formas de Pagamento.
        /// </summary>
        [XmlElement(nameof(pag))]
        public Pagamento[] pag { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo de Informações Adicionais.
        /// </summary>
        public InformacoesAdicionais infAdic { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Exportação.
        /// </summary>
        public Exportacao exporta { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Compra.
        /// </summary>
        public Compra compra { get; set; }

        /// <summary>
        /// (Opcional)
        /// Grupo Cana.
        /// </summary>
        public RegistroAquisicaoCana cana { get; set; }
    }
}
