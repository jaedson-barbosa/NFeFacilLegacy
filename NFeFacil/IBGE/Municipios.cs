using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.IBGE
{
    public static class Municipios
    {
        internal static Dictionary<Estado, IEnumerable<Municipio>> MunicipiosPorEstado;

        public static IEnumerable<Municipio> Buscar(Estado estado)
        {
            if (estado == null)
                throw new ArgumentNullException(nameof(estado));

            if (MunicipiosPorEstado == null)
                MunicipiosPorEstado = new Dictionary<Estado, IEnumerable<Municipio>>();

            if (!MunicipiosPorEstado.Keys.Contains(estado))
            {
                var xml = new XML(nameof(Municipios)).Retornar();
                var codigo = estado.Codigo.ToString(System.Globalization.CultureInfo.InvariantCulture);
                MunicipiosPorEstado.Add(estado,
                    from município in xml.Elements()
                    let proc = new ProcessamentoXml(município)
                    where proc.GetByIndex(0) == codigo
                    select new Municipio(município));
            }
            return MunicipiosPorEstado[estado];
        }

        public static IEnumerable<Municipio> Buscar(string sigla)
        {
            if (sigla == null)
                throw new ArgumentNullException(nameof(sigla));

            if (MunicipiosPorEstado == null)
                MunicipiosPorEstado = new Dictionary<Estado, IEnumerable<Municipio>>();

            var estado = Estados.Buscar().Single(x => x.Sigla == sigla);
            if (!MunicipiosPorEstado.Keys.Contains(estado))
            {
                var xml = new XML(nameof(Municipios)).Retornar();
                var codigo = estado.Codigo.ToString(System.Globalization.CultureInfo.InvariantCulture);
                MunicipiosPorEstado.Add(estado,
                    from município in xml.Elements()
                    let proc = new ProcessamentoXml(município)
                    where proc.GetByIndex(0) == codigo
                    select new Municipio(município));
            }
            return MunicipiosPorEstado[estado];
        }
    }
}
