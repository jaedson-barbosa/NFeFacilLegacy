using System;

namespace BaseGeral
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

        public static int ModoBuscaFornecedor
        {
            get => AssistenteConfig.Get(nameof(ModoBuscaFornecedor), 0);
            set => AssistenteConfig.Set(nameof(ModoBuscaFornecedor), value);
        }

        public static Guid IDBackgroung
        {
            get => AssistenteConfig.Get(nameof(IDBackgroung), Guid.Empty);
            set => AssistenteConfig.Set(nameof(IDBackgroung), value);
        }

        public static TiposBackground TipoBackground
        {
            get => (TiposBackground)AssistenteConfig.Get(nameof(TipoBackground), 2);
            set => AssistenteConfig.Set(nameof(TipoBackground), (int)value);
        }

        public static double OpacidadeBackground
        {
            get => AssistenteConfig.Get(nameof(OpacidadeBackground), 1d);
            set => AssistenteConfig.Set(nameof(OpacidadeBackground), value);
        }

        public static bool UsarSOAP12
        {
            get => AssistenteConfig.Get(nameof(UsarSOAP12), false);
            set => AssistenteConfig.Set(nameof(UsarSOAP12), value);
        }

        public static double LarguraDANFENFCe
        {
            get => AssistenteConfig.Get(nameof(LarguraDANFENFCe), 70d);
            set => AssistenteConfig.Set(nameof(LarguraDANFENFCe), value);
        }

        public static double MargemDANFENFCe
        {
            get => AssistenteConfig.Get(nameof(MargemDANFENFCe), 3d);
            set => AssistenteConfig.Set(nameof(MargemDANFENFCe), value);
        }

        public static SituacoesAlteracaoEstoque ConfiguracoesEstoque { get; set; } = new SituacoesAlteracaoEstoque();

        public static bool UsarFluent
        {
            get => AssistenteConfig.Get(nameof(UsarFluent), false);
            set => AssistenteConfig.Set(nameof(UsarFluent), value);
        }

        public static bool IgnorarProdutosJaAdicionados
        {
            get => AssistenteConfig.Get(nameof(IgnorarProdutosJaAdicionados), false);
            set => AssistenteConfig.Set(nameof(IgnorarProdutosJaAdicionados), value);
        }

        public static bool InformarResponsavelTecnico
        {
            get => AssistenteConfig.Get(nameof(InformarResponsavelTecnico), false);
            set => AssistenteConfig.Set(nameof(InformarResponsavelTecnico), value);
        }
    }

    public enum TiposBackground { Imagem, Cor, Padrao }

    public sealed class SituacoesAlteracaoEstoque
    {
        public SituacoesAlteracaoEstoque()
        {
            RV = ConfiguracoesEstoque / 1 << 7 == 1;
            RVCancel = ConfiguracoesEstoque / 1 << 6 == 1;
            NFeS = ConfiguracoesEstoque / 1 << 5 == 1;
            NFeSCancel = ConfiguracoesEstoque / 1 << 4 == 1;
            NFeE = ConfiguracoesEstoque / 1 << 3 == 1;
            NFeECancel = ConfiguracoesEstoque / 1 << 2 == 1;
            NFCe = ConfiguracoesEstoque / 1 << 1 == 1;
            NFCeCancel = ConfiguracoesEstoque / 1 << 0 == 1;
        }

        public bool RV { get; set; }
        public bool RVCancel { get; set; }
        public bool NFeS { get; set; }
        public bool NFeSCancel { get; set; }
        public bool NFeE { get; set; }
        public bool NFeECancel { get; set; }
        public bool NFCe { get; set; }
        public bool NFCeCancel { get; set; }

        static byte ConfiguracoesEstoque
        {
            get => AssistenteConfig.Get(nameof(ConfiguracoesEstoque), byte.MinValue);
            set => AssistenteConfig.Set(nameof(ConfiguracoesEstoque), value);
        }

        public void SalvarModificacoes()
        {
            int soma = 0;
            var bools = new bool[] { RV, RVCancel, NFeS, NFeSCancel, NFeE, NFeECancel, NFCe, NFCeCancel };
            for (int i = 7; i >= 0; i--) if (bools[i]) soma += 1 << i;
            ConfiguracoesEstoque = (byte)soma;
        }
    }
}
