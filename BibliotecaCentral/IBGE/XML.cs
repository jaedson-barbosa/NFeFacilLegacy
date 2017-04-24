using System.IO;
using System.Linq;
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

            var info = GetType().GetTypeInfo();
            assembly = info.Assembly;
        }

        public XElement Retornar()
        {
            var str = GetStreamRecurso("BibliotecaCentral.IBGE." + NomeArquivo);
            return XElement.Load(str);
        }

        private Stream GetStreamRecurso(string uri)
        {
            var recursos = assembly.GetManifestResourceNames();
            if (recursos.Contains(uri))
                return assembly.GetManifestResourceStream(uri);
            else
                throw new FileNotFoundException("O arquivo especificado não está presente no assembly atual.", uri);
        }
    }
}
