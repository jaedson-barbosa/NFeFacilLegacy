using System.Xml.Linq;

namespace BibliotecaCentral.IBGE
{
    public class Estado
    {
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public ushort Codigo { get; set; }

        public Estado() { }

        public Estado(string nome, string sigla, string codigo)
        {
            Nome = nome;
            Sigla = sigla;
            Codigo = ushort.Parse(codigo);
        }

        public Estado(XElement xmlEstado)
        {
            ProcessamentoXml proc = xmlEstado;
            Nome = proc.GetByName(nameof(Nome));
            Sigla = proc.GetByName(nameof(Sigla));
            Codigo = ushort.Parse(proc.GetByName(nameof(Codigo)));
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
