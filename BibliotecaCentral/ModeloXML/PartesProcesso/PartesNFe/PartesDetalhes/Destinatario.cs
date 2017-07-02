﻿using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class Destinatario
    {
        [XmlElement(Order = 0)]
        public string CPF { get; set; }

        [XmlElement(Order = 1)]
        public string CNPJ { get; set; }

        [XmlElement("idEstrangeiro", Order = 2)]
        public string IdEstrangeiro { get; set; }

        [XmlElement(ElementName = "xNome", Order = 3)]
        public string Nome { get; set; }

        [XmlElement(ElementName = "enderDest", Order = 4)]
        public EnderecoCompleto Endereco { get; set; } = new EnderecoCompleto();

        [XmlElement(ElementName = "indIEDest", Order = 5)]
        public int IndicadorIE { get; set; } = 9;

        [XmlElement(ElementName = "IE", Order = 6)]
        public string InscricaoEstadual { get; set; }

        [XmlElement(Order = 7)]
        public string ISUF { get; set; }

        [XmlElement("email", Order = 8)]
        public string Email { get; set; }

        [XmlIgnore]
        public string Documento => IdEstrangeiro ?? CNPJ ?? CPF;
    }
}
