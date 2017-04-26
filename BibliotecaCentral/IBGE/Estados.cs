using System.Collections.Generic;
using System.Linq;

namespace BibliotecaCentral.IBGE
{
    public static class Estados
    {
        public static IEnumerable<Estado> EstadosCache;

        internal static void Buscar()
        {
            if (EstadosCache == null)
            {
                var xml = new XML(nameof(Estados)).Retornar();
                EstadosCache = from estado in xml.Elements()
                               select new Estado(estado);
            }
        }
    }
}
