using System.Linq;

namespace BibliotecaCentral.IBGE
{
    public static class Estados
    {
        public static Estado[] EstadosCache;

        internal static void Buscar()
        {
            if (EstadosCache == null)
            {
                var xml = new XML(nameof(Estados)).Retornar();
                var filhos = xml.Elements().ToArray();
                var quant = filhos.Length;
                EstadosCache = new Estado[quant];
                for (int i = 0; i < quant; i++)
                {
                    EstadosCache[i] = new Estado(filhos[i]);
                }
            }
        }

        public static Estado Buscar(ushort codigo) => EstadosCache.First(x => x.Codigo == codigo);
        public static Estado Buscar(string siglaOuNome)
        {
            bool isSigla = siglaOuNome.Length == 2;
            return EstadosCache.First(x => isSigla ? x.Sigla == siglaOuNome : x.Nome == siglaOuNome);
        }
    }
}
