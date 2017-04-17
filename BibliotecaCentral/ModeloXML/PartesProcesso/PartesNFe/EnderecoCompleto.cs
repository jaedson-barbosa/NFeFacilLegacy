using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe
{
    public sealed class enderecoCompleto : enderecoBase
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
        public int Id { get; set; }
        public string CEP { get; set; }
        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;
        [XmlElement("xPais")]
        public string XPais { get; set; } = "Brasil";
        [XmlElement(ElementName = "fone")]
        public string Telefone { get; set; }
    }
}
