using NFeFacil.ItensBD;
using System;
using Windows.UI.Xaml.Media;

namespace NFeFacil
{
    public static class DefinicoesTemporarias
    {
        public static EmitenteDI EmitenteAtivo { get; set; }
        public static ImageSource Logotipo { get; set; }
        public static Vendedor VendedorAtivo { get; set; }
        public static ImageSource FotoVendedor { get; set; }

        public static DateTime DateTimeNow
        {
            get
            {
                var atual = DateTime.Now;
                if (atual.IsDaylightSavingTime() && DefinicoesPermanentes.SuprimirHorarioVerao)
                {
                    return atual.AddHours(-1);
                }
                return atual;
            }
        }

        public static DateTimeOffset DateTimeOffsetNow
        {
            get
            {
                var atual = DateTime.Now;
                if (atual.IsDaylightSavingTime() && DefinicoesPermanentes.SuprimirHorarioVerao)
                {
                    return atual.AddHours(-1);
                }
                return atual;
            }
        }
    }
}
