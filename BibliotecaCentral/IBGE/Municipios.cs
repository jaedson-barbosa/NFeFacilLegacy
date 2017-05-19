using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BibliotecaCentral.IBGE
{
    public static class Municipios
    {
        public static IEnumerable<Municipio> Get(Estado est)
        {
            if (est == null || est.Codigo == 0) return new ObservableCollection<Municipio>();
            var estados = Estados.EstadosCache;
            var MunicipiosCache = from município in new XML(nameof(Municipios)).Retornar().Elements()
                                  let mun = new Municipio(município)
                                  group mun by estados.First(x => x.Codigo == mun.CodigoUF);
            return MunicipiosCache.First(x => x.Key == est);
        }

        public static IEnumerable<Municipio> Get(string nomeSigla)
        {
            if (string.IsNullOrEmpty(nomeSigla)) return new ObservableCollection<Municipio>();
            var estados = Estados.EstadosCache;
            var MunicipiosCache = from município in new XML(nameof(Municipios)).Retornar().Elements()
                                  let mun = new Municipio(município)
                                  group mun by estados.First(x => x.Codigo == mun.CodigoUF);
            return MunicipiosCache.First(x => (nomeSigla.Length == 2 ? x.Key.Sigla : x.Key.Nome) == nomeSigla);
        }

        public static IEnumerable<Municipio> Get(ushort codigo)
        {
            if (codigo == 0) return new ObservableCollection<Municipio>();
            var MunicipiosCache = from município in new XML(nameof(Municipios)).Retornar().Elements()
                                  let mun = new Municipio(município)
                                  group mun by mun.CodigoUF;
            return MunicipiosCache.First(x => x.Key == codigo);
        }
    }
}
