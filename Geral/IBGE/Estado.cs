using System.Xml.Linq;

namespace BaseGeral.IBGE
{
    public sealed class Estado
    {
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public ushort Codigo { get; set; }

        public Estado(XElement xmlEstado)
        {
            Nome = xmlEstado.Element(nameof(Nome)).Value;
            Sigla = xmlEstado.Element(nameof(Sigla)).Value;
            Codigo = ushort.Parse(xmlEstado.Element(nameof(Codigo)).Value);
        }

        public override bool Equals(object obj) => obj is Estado est ? Codigo == est.Codigo : false;
        public override int GetHashCode() => Codigo;
    }
}
