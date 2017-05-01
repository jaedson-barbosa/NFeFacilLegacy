using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService
{
    internal static class EnderecosConexao
    {
        private static string ObterEndereco(bool ambienteTestes, string siglaUF, Operacoes operacaoSolicitada)
        {
            string[] SVAN = { "MA", "PA", "PI" };
            string[] SVRS = { "AC", "AL", "AP", "DF", "ES", "PB", "RJ", "RN", "RO", "RR", "SC", "SE", "TO" };

            if (ambienteTestes)
            {
                if (SVAN.Contains(siglaUF))
                {
                    
                }
                else if (SVRS.Contains(siglaUF))
                {

                }
                else
                {

                }
            }
            else
            {
                if (SVAN.Contains(siglaUF))
                {

                }
                else if (SVRS.Contains(siglaUF))
                {

                }
                else
                {

                }
            }
        }
    }

    internal enum Operacoes
    {
        Consultar,
        Autorizar,
        RespostaAutorizar
    }
}
