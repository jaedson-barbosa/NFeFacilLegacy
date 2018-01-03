using System;
using Windows.Storage;

namespace NFeFacil
{
    public static class DefinicoesPermanentes
    {
        static ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;

        public static bool SuprimirHorarioVerao
        {
            get
            {
                var tipo = Pasta.Values[nameof(SuprimirHorarioVerao)];
                return tipo == null ? SuprimirHorarioVerao = false : (bool)tipo;
            }
            set => Pasta.Values[nameof(SuprimirHorarioVerao)] = value;
        }

        public static bool CalcularNumeroNFe
        {
            get
            {
                var tipo = Pasta.Values[nameof(CalcularNumeroNFe)];
                return tipo == null ? CalcularNumeroNFe = true : (bool)tipo;
            }
            set => Pasta.Values[nameof(CalcularNumeroNFe)] = value;
        }

        public static int ModoBuscaProduto
        {
            get
            {
                var tipo = Pasta.Values[nameof(ModoBuscaProduto)];
                return tipo == null ? 0 : (int)tipo;
            }
            set => Pasta.Values[nameof(ModoBuscaProduto)] = value;
        }

        internal static Guid IDBackgroung
        {
            get
            {
                var valor = Pasta.Values["IDBackgroung"];
                return valor != null ? (Guid)valor : Guid.Empty;
            }
            set => Pasta.Values["IDBackgroung"] = value;
        }

        internal static TiposBackground TipoBackground
        {
            get
            {
                var atual = Pasta.Values[nameof(TipoBackground)];
                return atual == null ? TiposBackground.Padrao : (TiposBackground)atual;
            }
            set => Pasta.Values[nameof(TipoBackground)] = (int)value;
        }

        internal static double OpacidadeBackground
        {
            get
            {
                var atual = Pasta.Values[nameof(OpacidadeBackground)];
                return atual == null ? 1 : (double)atual;
            }
            set => Pasta.Values[nameof(OpacidadeBackground)] = value;
        }
    }

    internal enum TiposBackground { Imagem, Cor, Padrao }
}
