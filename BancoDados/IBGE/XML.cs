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
            using (var stream = Extensoes.Retornar(this, "BancoDados.IBGE." + NomeArquivo))
            {
                return XElement.Load(stream);
            }
        }
    }
}
