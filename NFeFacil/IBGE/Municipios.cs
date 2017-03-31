using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.IBGE
{
    public static class Municipios
    {
        private static Dictionary<Estado, IEnumerable<Municipio>> MunicipiosCache;

        public static IEnumerable<Municipio> Get(Estado est)
        {
            return MunicipiosCache[est];
        }

        public static IEnumerable<Municipio> Get(ushort codigo)
        {
            return MunicipiosCache.First(x => x.Key.Codigo == codigo).Value;
        }

        public static IEnumerable<Municipio> Get(string nomeSigla)
        {
            return MunicipiosCache.First(x => (nomeSigla.Length == 2 ? x.Key.Sigla : x.Key.Nome) == nomeSigla).Value;
        }

        public static void Buscar()
        {
            if (MunicipiosCache == null)
            {
                MunicipiosCache = new Dictionary<Estado, IEnumerable<Municipio>>();
                var xml = new XML(nameof(Municipios)).Retornar();
                var municipios = from município in xml.Elements()
                                 let proc = new ProcessamentoXml(município)
                                 select new Municipio(município);
                foreach (var item in Estados.EstadosCache)
                {
                    MunicipiosCache.Add(item, from mun in municipios
                                              where mun.CodigoUF == item.Codigo
                                              select mun);
                }
            }
        }
    }
}
