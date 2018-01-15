using System;

namespace NFeFacil.Sincronizacao
{
    public static class ConfiguracoesSincronizacao
    {
        public static TipoAppSincronizacao Tipo
        {
            get => (TipoAppSincronizacao)AssistenteConfig.Get(nameof(Tipo), 1);
            set
            {
                AssistenteConfig.Set(nameof(Tipo), (int)value);
                SenhaPermanente = default(int);
                SenhaTemporária = default(int);
                IPServidor = null;
            }
        }

        public static DateTime UltimaSincronizacao
        {
            get => DateTime.FromBinary(AssistenteConfig.Get<long>(nameof(UltimaSincronizacao), 0));
            set => AssistenteConfig.Set(nameof(UltimaSincronizacao), value.ToBinary());
        }

        public static DateTime UltimaSincronizacaoNotas
        {
            get => DateTime.FromBinary(AssistenteConfig.Get<long>(nameof(UltimaSincronizacaoNotas), 0));
            set => AssistenteConfig.Set(nameof(UltimaSincronizacaoNotas), value.ToBinary());
        }

        public static int SenhaPermanente
        {
            get
            {
                var senha = AssistenteConfig.Get(nameof(SenhaPermanente), 0);
                return senha != 0 ? senha : SenhaPermanente = new Random().Next(10000, 100000);
            }
            set => AssistenteConfig.Set(nameof(SenhaPermanente), value);
        }

        public static int SenhaTemporária { get; set; }

        public static string IPServidor
        {
            get => AssistenteConfig.Get<string>(nameof(IPServidor), null);
            set => AssistenteConfig.Set(nameof(IPServidor), value);
        }

        public static bool InícioAutomático
        {
            get => AssistenteConfig.Get(nameof(InícioAutomático), false);
            set => AssistenteConfig.Set(nameof(InícioAutomático), value);
        }
    }
}
