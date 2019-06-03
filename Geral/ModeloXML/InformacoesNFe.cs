using BaseGeral.View;
using BaseGeral.ModeloXML.PartesDetalhes;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BaseGeral.ModeloXML
{
    [XmlRoot("infNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class InformacoesNFe : InformacoesBase
    {
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

        [DescricaoPropriedade("Formas de pagamento")]
        [XmlElement("pag", Namespace = "http://www.portalfiscal.inf.br/nfe", Order = 9)]
        public DetalhamentoPagamento Pagamento { get; set; }

        [DescricaoPropriedade("Informações Adicionais")]
        [XmlElement(Order = 10)]
        public InformacoesAdicionais infAdic { get; set; }

        [DescricaoPropriedade("Cana de açúcar")]
        [XmlElement(Order = 13)]
        public RegistroAquisicaoCana cana { get; set; }

        ResponsavelTecnico responsavel;
        [DescricaoPropriedade("Responsável técnico")]
        [XmlElement("infRespTec", Order = 14)]
        public ResponsavelTecnico Responsavel
        {
            get => DefinicoesPermanentes.InformarResponsavelTecnico
                ? responsavel ?? (responsavel = new ResponsavelTecnico().PreencherPadrao())
                : null;
            set => responsavel = value;
        }
    }

    public sealed class ResponsavelTecnico
    {
        [XmlElement("CNPJ", Order = 0), DescricaoPropriedade("CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement("xContato", Order = 0), DescricaoPropriedade("Nome para contato")]
        public string Contato { get; set; }

        [XmlElement("email", Order = 0), DescricaoPropriedade("E-mail para contato")]
        public string Email { get; set; }

        [XmlElement("fone", Order = 0), DescricaoPropriedade("Fone para contato")]
        public string Fone { get; set; }

        public ResponsavelTecnico PreencherPadrao()
        {
            CNPJ = "12931158000164";
            Contato = "Jaedson Barbosa Serafim";
            Email = "jaedson33@gmail.com";
            Fone = "83988856440";
            return this;
        }
    }
}
