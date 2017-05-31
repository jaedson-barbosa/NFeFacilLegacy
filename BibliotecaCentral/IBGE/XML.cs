using System.Reflection;
using System.Xml.Linq;

namespace BibliotecaCentral.IBGE
{
    internal sealed class XML
    {
        private string NomeArquivo;
        private Assembly assembly;

        public XML(string nome)
        {
            if (!nome.Contains(".xml")) nome += ".xml";
            NomeArquivo = nome;
            assembly = GetType().GetTypeInfo().Assembly;
        }

        public XElement Retornar()
        {
            using (var stream = assembly.GetManifestResourceStream("BibliotecaCentral.IBGE." + NomeArquivo))
            {
                return XElement.Load(stream);
            }
        }
    }
}
