using NFeFacil.ItensBD;
using System;
using Windows.Storage;

namespace NFeFacil
{
    public static class Propriedades
    {
        public static EmitenteDI EmitenteAtivo { get; set; }
        public static Vendedor VendedorAtivo { get; set; }

        internal static DateTime DateTimeNow
        {
            get
            {
                var atual = DateTime.Now;
                if (atual.IsDaylightSavingTime() && Configuracoes.SuprimirHorarioVerao)
                {
                    return atual.AddHours(-1);
                }
                return atual;
            }
        }

        internal static DateTimeOffset DateTimeOffsetNow
        {
            get
            {
                var atual = DateTime.Now;
                if (atual.IsDaylightSavingTime() && Configuracoes.SuprimirHorarioVerao)
                {
                    return atual.AddHours(-1);
                }
                return atual;
            }
        }
    }

    public static class Configuracoes
    {
        static ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;

        public static bool SuprimirHorarioVerao
    {
            get
            {
                var tipo = Pasta.Values[nameof(SuprimirHorarioVerao)];
                return tipo == null ? SuprimirHorarioVerao = false : (bool)tipo;
            }
            set
            {
                Pasta.Values[nameof(SuprimirHorarioVerao)] = value;
            }
        }
    }
}
