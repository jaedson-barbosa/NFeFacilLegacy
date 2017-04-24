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
        public Identificacao identificação;

        [XmlElement(ElementName = "emit")]
        public Emitente emitente;

        [XmlElement("dest")]
        public Destinatario destinatário;

        /// <summary>
        /// (Opcional)
        /// Identificação do Local de retirada.
        /// </summary>
        public RetiradaOuEntrega retirada;

        /// <summary>
        /// (Opcional)
        /// Identificação do Local de entrega.
        /// </summary>
        public RetiradaOuEntrega entrega;

        [XmlElement(ElementName = "det", Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public List<DetalhesProdutos> produtos;

        public Total total;

        public Transporte transp;

        /// <summary>
        /// (Opcional)
        /// Grupo Cobrança.
        /// </summary>
        public Cobranca cobr;

        /// <summary>
        /// (Opcional)
        /// Grupo de Formas de Pagamento.
        /// </summary>
        [XmlElement(nameof(pag))]
        public Pagamento[] pag;

        /// <summary>
        /// (Opcional)
        /// Grupo de Informações Adicionais.
        /// </summary>
        public InformacoesAdicionais infAdic;

        /// <summary>
        /// (Opcional)
        /// Grupo Exportação.
        /// </summary>
        public Exportacao exporta;

        /// <summary>
        /// (Opcional)
        /// Grupo Compra.
        /// </summary>
        public Compra compra;

        /// <summary>
        /// (Opcional)
        /// Grupo Cana.
        /// </summary>
        public RegistroAquisicaoCana cana;
    }
}
