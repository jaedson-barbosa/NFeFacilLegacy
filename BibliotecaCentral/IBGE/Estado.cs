using System.Xml.Linq;

namespace BibliotecaCentral.IBGE
{
    public struct Estado
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

        public static bool operator ==(Estado est1, Estado est2)
        {
            if ((object)est1 == null && (object)est2 == null) return true;
            else if ((object)est1 == null || (object)est2 == null) return false;
            else return est1.Equals(est2);
        }
        public static bool operator != (Estado est1, Estado est2) => !(est1 == est2);

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
            return Nome.GetHashCode() + Sigla.GetHashCode() + Codigo.GetHashCode();
        }
    }
}
