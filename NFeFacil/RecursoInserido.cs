using System.IO;
using System.Reflection;

namespace NFeFacil
{
    internal class RecursoInserido
    {
        public Stream Retornar(string caminho)
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream(caminho);
        }
    }
}
