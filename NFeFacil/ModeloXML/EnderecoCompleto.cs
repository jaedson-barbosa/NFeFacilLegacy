using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML
{
    public sealed class EnderecoCompleto : IEnderecoCompleto
    {
        public EnderecoCompleto() { }

        public EnderecoCompleto(string logradouro, string número, string bairro, int codigoMunicípio, string nomeMunicípio, string siglaUF, string Cep = null, string telefone = null, string complemento = null)
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

        [XmlElement("xLgr", Order = 0)]
        public string Logradouro { get; set; }

        [DescricaoPropriedade("Número")]
        [XmlElement("nro", Order = 1)]
        public string Numero { get; set; }

        [XmlElement("xCpl", Order = 2)]
        public string Complemento { get; set; }

        [XmlElement("xBairro", Order = 3)]
        public string Bairro { get; set; }

        [DescricaoPropriedade("Código do município")]
        [XmlElement("cMun", Order = 4)]
        public int CodigoMunicipio { get; set; }

        [DescricaoPropriedade("Nume do município")]
        [XmlElement("xMun", Order = 5)]
        public string NomeMunicipio { get; set; }

        [PropriedadeExtensivel("Estado", MetodosObtencao.Estado)]
        [XmlElement("UF", Order = 6)]
        public string SiglaUF { get; set; }

        [XmlElement(Order = 7)]
        public string CEP { get; set; }

        [DescricaoPropriedade("Código do país")]
        [XmlElement("cPais", Order = 8)]
        public int CPais { get; set; } = 1058;

        [DescricaoPropriedade("Nome do país")]
        [XmlElement("xPais", Order = 9)]
        public string XPais { get; set; } = "Brasil";

        [XmlElement(ElementName = "fone", Order = 10)]
        public string Telefone { get; set; }
    }
}
