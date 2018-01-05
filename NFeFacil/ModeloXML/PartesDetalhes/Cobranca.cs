using NFeFacil.View;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesDetalhes
{
    public sealed class Cobranca
    {
        [XmlElement("fat"), DescricaoPropriedade("Fatura")]
        public Fatura Fat { get; set; } = new Fatura();

        [XmlElement("dup")]
        public List<Duplicata> Dup { get; set; } = new List<Duplicata>();
    }
}
