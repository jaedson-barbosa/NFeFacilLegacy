﻿using System;
using Windows.Storage;

namespace NFeFacil
{
    public static class ConfiguracoesPermanentes
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
