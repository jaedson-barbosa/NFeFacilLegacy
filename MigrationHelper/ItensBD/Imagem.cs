using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;

namespace BaseGeral.ItensBD
{
    public sealed class Imagem : IUltimaData, IGuidId
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }
        [XmlIgnore]
        public byte[] Bytes { get; set; }
        [NotMapped]
        public string BytesXML
        {
            get => string.Concat(Bytes.Select(x => x.ToString("000 ")));
            set
            {
                var ret = Bytes = value.TrimEnd().Split(' ').Select(x => byte.Parse(x)).ToArray();
                Bytes = ret;
            }
        }
    }
}
