using Microsoft.EntityFrameworkCore;

namespace NFeFacil
{
    public static class InicioGeral
    {
        public static void IniciarBancoDados()
        {
            using (var db = new AplicativoContext())
            {
                db.Database.Migrate();
            }
        }

        public static void IniciarIBGE()
        {
            IBGE.Estados.Buscar();
            IBGE.Municipios.Buscar();
        }
    }
}
