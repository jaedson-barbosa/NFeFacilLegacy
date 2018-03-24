using System.Linq;

namespace BaseGeral.IBGE
{
    public static class Estados
    {
        public static Estado[] EstadosCache;

        public static void Buscar()
        {
            if (EstadosCache == null)
            {
                var xml = new XML(nameof(Estados)).Retornar();
                EstadosCache = xml.Elements().Select(x => new Estado(x)).ToArray();
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
