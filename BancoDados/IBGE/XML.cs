using System.Reflection;
using System.Xml.Linq;

namespace NFeFacil.IBGE
{
    internal sealed class XML
    {
        private string NomeArquivo;

        public XML(string nome)
        {
            if (!nome.Contains(".xml")) nome += ".xml";
            NomeArquivo = nome;
        }

        public XElement Retornar()
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("BancoDados.IBGE." + NomeArquivo))
            {
                return XElement.Load(stream);
            }
        }
    }
}
