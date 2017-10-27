using System;
using Windows.Storage;

namespace NFeFacil.Sincronizacao
{
    public static class ConfiguracoesSincronizacao
    {
        private static ApplicationDataContainer Pasta = ApplicationData.Current.LocalSettings;

        public static TipoAppSincronizacao Tipo
        {
            get
            {
                var tipo = Pasta.Values[nameof(Tipo)];
                return tipo == null ? Tipo = TipoAppSincronizacao.Cliente : (TipoAppSincronizacao)tipo;
            }
            set
            {
                Pasta.Values[nameof(Tipo)] = (int)value;
                SenhaPermanente = default(int);
                SenhaTemporária = default(int);
                IPServidor = null;
            }
        }

        public static DateTime UltimaSincronizacao
        {
            get
            {
                var valor = Pasta.Values[nameof(UltimaSincronizacao)];
                if (valor == null) return DateTime.MinValue;
                else return DateTime.FromBinary((long)valor);
            }
            set
            {
                Pasta.Values[nameof(UltimaSincronizacao)] = value.ToBinary();
            }
        }

        public static DateTime UltimaSincronizacaoNotas
        {
            get
            {
                var valor = Pasta.Values[nameof(UltimaSincronizacaoNotas)];
                if (valor == null) return DateTime.MinValue;
                else return DateTime.FromBinary((long)valor);
            }
            set
            {
                Pasta.Values[nameof(UltimaSincronizacaoNotas)] = value.ToBinary();
            }
        }

        public static int SenhaPermanente
        {
            get
            {
                var senha = Pasta.Values[nameof(SenhaPermanente)];
                if (senha == null || (int)senha == 0)
                {
                    var random = new Random();
                    return SenhaPermanente = random.Next(10000, 100000);
                }
                else
                {
                    return (int)senha;
                }
            }
            set
            {
                Pasta.Values[nameof(SenhaPermanente)] = value;
            }
        }

        public static int SenhaTemporária { get; set; }

        public static string IPServidor
        {
            get { return (string)Pasta.Values[nameof(IPServidor)]; }
            set { Pasta.Values[nameof(IPServidor)] = value; }
        }

        public static bool InícioAutomático
        {
            get
            {
                var tipo = Pasta.Values[nameof(InícioAutomático)];
                return tipo == null ? InícioAutomático = false : (bool)tipo;
            }
            set
            {
                Pasta.Values[nameof(InícioAutomático)] = value;
            }
        }
    }
}
