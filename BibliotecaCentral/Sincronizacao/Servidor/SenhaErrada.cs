using System;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    public class SenhaErrada : Exception
    {
        public int SenhaInformada { get; set; }

        public SenhaErrada() { }
        public SenhaErrada(int senha) : base("Foi informada a senha errada para conexão com o servidor")
        {
            SenhaInformada = senha;
        }
    }
}
