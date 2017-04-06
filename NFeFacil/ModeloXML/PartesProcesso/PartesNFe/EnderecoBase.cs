using NFeFacil.IBGE;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe
{
    public abstract class enderecoBase
    {
        [XmlElement("xLgr")]
        public string Logradouro { get; set; }
        [XmlElement("nro")]
        public string Numero { get; set; }
        [XmlElement("xCpl")]
        public string Complemento { get; set; }
        [XmlElement("xBairro")]
        public string Bairro { get; set; }
        [XmlElement("cMun")]
        public long CodigoMunicipio { get; set; }
        [XmlElement("xMun")]
        public string NomeMunicipio { get; set; }
        [XmlElement("UF")]
        public string SiglaUF { get; set; }
        [NotMapped]
        [XmlIgnore]
        public Municipio ConjuntoMunicipio
        {
            get
            {
                if (CodigoMunicipio > 0)
                {
                    return Municipios.Get(SiglaUF).First(x => x.CodigoMunicípio == CodigoMunicipio);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                NomeMunicipio = value.Nome;
                CodigoMunicipio = value.CodigoMunicípio;
            }
        }
    }
}
