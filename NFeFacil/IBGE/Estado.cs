using System.Xml.Linq;

namespace NFeFacil.IBGE
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
    }
}
