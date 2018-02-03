using System;

namespace NFeFacil
{
    public static class DefinicoesPermanentes
    {
        public static bool SuprimirHorarioVerao
        {
            get => AssistenteConfig.Get(nameof(SuprimirHorarioVerao), false);
            set => AssistenteConfig.Set(nameof(SuprimirHorarioVerao), value);
        }

        public static bool CalcularNumeroNFe
        {
            get => AssistenteConfig.Get(nameof(CalcularNumeroNFe), true);
            set => AssistenteConfig.Set(nameof(CalcularNumeroNFe), value);
        }

        public static int ModoBuscaProduto
        {
            get => AssistenteConfig.Get(nameof(ModoBuscaProduto), 0);
            set => AssistenteConfig.Set(nameof(ModoBuscaProduto), value);
        }

        public static int ModoBuscaCliente
        {
            get => AssistenteConfig.Get(nameof(ModoBuscaCliente), 0);
            set => AssistenteConfig.Set(nameof(ModoBuscaCliente), value);
        }

        public static int ModoBuscaComprador
        {
            get => AssistenteConfig.Get(nameof(ModoBuscaComprador), 0);
            set => AssistenteConfig.Set(nameof(ModoBuscaComprador), value);
        }

        public static int ModoBuscaMotorista
        {
            get => AssistenteConfig.Get(nameof(ModoBuscaMotorista), 0);
            set => AssistenteConfig.Set(nameof(ModoBuscaMotorista), value);
        }

        public static int ModoBuscaVendedor
        {
            get => AssistenteConfig.Get(nameof(ModoBuscaVendedor), 0);
            set => AssistenteConfig.Set(nameof(ModoBuscaVendedor), value);
        }
        
        internal static Guid IDBackgroung
        {
            get => AssistenteConfig.Get(nameof(IDBackgroung), Guid.Empty);
            set => AssistenteConfig.Set(nameof(IDBackgroung), value);
        }

        internal static TiposBackground TipoBackground
        {
            get => AssistenteConfig.Get(nameof(TipoBackground), TiposBackground.Padrao);
            set => AssistenteConfig.Set(nameof(TipoBackground), value);
        }

        internal static double OpacidadeBackground
        {
            get => AssistenteConfig.Get(nameof(OpacidadeBackground), 1);
            set => AssistenteConfig.Set(nameof(OpacidadeBackground), value);
        }

        internal static bool UsarSOAP12
        {
            get => AssistenteConfig.Get(nameof(UsarSOAP12), false);
            set => AssistenteConfig.Set(nameof(UsarSOAP12), value);
        }
    }

    internal enum TiposBackground { Imagem, Cor, Padrao }
}
