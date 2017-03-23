using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.IBGE
{
    public static class Estados
    {
        internal static IEnumerable<Estado> EstadosCache;

        public static IEnumerable<Estado> Buscar()
        {
            if (EstadosCache == null)
            {
                var classe = new XML(nameof(Estados));
                var xml = classe.Retornar();
                EstadosCache = from estado in xml.Elements()
                                     select new Estado(estado);
            }
            return EstadosCache;
        }
    }
}
