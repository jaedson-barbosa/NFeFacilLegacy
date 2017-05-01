using System;
using Windows.Storage;

namespace BibliotecaCentral.Configuracoes
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
                SincDadoBase = default(bool);
                SincNotaFiscal = default(bool);
                SenhaPermanente = default(int);
                SenhaTemporária = default(int);
                IPServidor = null;
            }
        }

        public static bool SincDadoBase
        {
            get
            {
                var tipo = Pasta.Values[nameof(SincDadoBase)];
                return tipo == null ? SincDadoBase = false : (bool)tipo;
            }
            set
            {
                Pasta.Values[nameof(SincDadoBase)] = value;
            }
        }

        public static bool SincNotaFiscal
        {
            get
            {
                var tipo = Pasta.Values[nameof(SincNotaFiscal)];
                return tipo == null ? SincNotaFiscal = false : (bool)tipo;
            }
            set
            {
                Pasta.Values[nameof(SincNotaFiscal)] = value;
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

        public static int SenhaTemporária
        {
            get { return (int)Pasta.Values[nameof(SenhaTemporária)]; }
            set { Pasta.Values[nameof(SenhaTemporária)] = value; }
        }

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

        public static bool SincronizarAutomaticamente
        {
            get
            {
                var tipo = Pasta.Values[nameof(SincronizarAutomaticamente)];
                return tipo == null ? SincronizarAutomaticamente = false : (bool)tipo;
            }
            set
            {
                Pasta.Values[nameof(SincronizarAutomaticamente)] = value;
            }
        }
    }
}
