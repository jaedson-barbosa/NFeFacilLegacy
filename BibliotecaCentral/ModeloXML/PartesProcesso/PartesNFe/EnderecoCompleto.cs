using System;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe
{
    public sealed class enderecoCompleto
    {
        public enderecoCompleto() { }

        public enderecoCompleto(string logradouro, string número, string bairro, int codigoMunicípio, string nomeMunicípio, string siglaUF, string Cep = null, string telefone = null, string complemento = null)
        {
            Logradouro = logradouro;
            Numero = número;
            Complemento = complemento;
            Bairro = bairro;
            CodigoMunicipio = codigoMunicípio;
            NomeMunicipio = nomeMunicípio;
            SiglaUF = siglaUF;
            CEP = Cep;
            Telefone = telefone;
        }

        public enderecoCompleto(enderecoCompleto other) : this(other.Logradouro, other.Numero, other.Bairro, other.CodigoMunicipio, other.NomeMunicipio, other.SiglaUF, other.CEP, other.Telefone, other.Complemento) { }

        [XmlIgnore]
        public Guid Id { get; set; }
        [XmlElement("xLgr")]
        public string Logradouro { get; set; }
        [XmlElement("nro")]
        public string Numero { get; set; }
        [XmlElement("xCpl")]
        public string Complemento { get; set; }
        [XmlElement("xBairro")]
        public string Bairro { get; set; }
        [XmlElement("cMun")]
        public int CodigoMunicipio { get; set; }
        [XmlElement("xMun")]
        public string NomeMunicipio { get; set; }
        [XmlElement("UF")]
        public string SiglaUF { get; set; }
        public string CEP { get; set; }
        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;
        [XmlElement("xPais")]
        public string XPais { get; set; } = "Brasil";
        [XmlElement(ElementName = "fone")]
        public string Telefone { get; set; }
    }
}
