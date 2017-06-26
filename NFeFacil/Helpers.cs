using BibliotecaCentral.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFeFacil
{
    static class Helpers
    {
        static ILog Log = new Popup();

        public static void ManipularErro(this Exception erro)
        {
            Log.Escrever(TitulosComuns.ErroSimples, erro.Message);
        }
    }
}
