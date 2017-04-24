using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;

namespace NFeFacil
{
    internal static class Propriedades
    {
        internal static IntercambioTelas Intercambio { get; set; }
        internal static GerenciadorServidor Server { get; set; } = new GerenciadorServidor(new Saida());

        private static User usuario;
        internal static User Usuario
        {
            get
            {
                if (usuario == null)
                {
                    var usuarios = from user in User.FindAllAsync(UserType.LocalUser).GetResults()
                                   where user.AuthenticationStatus == UserAuthenticationStatus.LocallyAuthenticated
                                   select user;
                    usuario = usuarios.First();
                }
                return usuario;
            }
            set => usuario = value;
        }

        internal static async Task<string> ObterNomeAsync(this User usuario)
        {
            string primeiroNome = await usuario.GetPropertyAsync(KnownUserProperties.FirstName) as string;
            primeiroNome = char.ToUpper(primeiroNome[0]) + primeiroNome.Substring(1);
            string ultimoNome = await usuario.GetPropertyAsync(KnownUserProperties.LastName) as string;
            ultimoNome = char.ToUpper(ultimoNome[0]) + ultimoNome.Substring(1);
            return $"{primeiroNome} {ultimoNome}";
        }
    }
}
