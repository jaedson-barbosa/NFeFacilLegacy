using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe
{
    public sealed class EnderecoCompleto : EnderecoBase
    {
        public EnderecoCompleto() { }

        public EnderecoCompleto(string logradouro, string número, string bairro, long codigoMunicípio, string nomeMunicípio, string siglaUF, string Cep = null, string telefone = null, string complemento = null)
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

        public EnderecoCompleto(EnderecoCompleto other) : this(other.Logradouro, other.Numero, other.Bairro, other.CodigoMunicipio, other.NomeMunicipio, other.SiglaUF, other.CEP, other.Telefone, other.Complemento) { }

        public string CEP { get; set; }
        [XmlElement("cPais")]
        public int CPais { get; set; } = 1058;
        [XmlElement("xPais")]
        public string XPais { get; set; } = "Brasil";
        [XmlElement(ElementName = "fone")]
        public string Telefone { get; set; }
    }
}
