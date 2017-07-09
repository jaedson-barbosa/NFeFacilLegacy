using BibliotecaCentral.Log;
using System;

namespace NFeFacil
{
    static class Helpers
    {
        static ILog Log = Popup.Current;

        public static void ManipularErro(this Exception erro)
        {
            Log.Escrever(TitulosComuns.Erro, erro.Message);
        }
    }
}
