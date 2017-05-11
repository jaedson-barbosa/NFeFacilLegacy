using System.Xml.Linq;

namespace BibliotecaCentral.IBGE
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

        public override bool Equals(object obj)
        {
            if (obj is Estado est)
            {
                return GetHashCode() == est.GetHashCode();
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Nome?.GetHashCode() ?? 0 + Sigla?.GetHashCode() ?? 0 + Codigo.GetHashCode();
        }
    }
}
