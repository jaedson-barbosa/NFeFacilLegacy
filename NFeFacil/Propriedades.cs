using NFeFacil.Log;
using NFeFacil.NavegacaoUI;
using NFeFacil.Sincronizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFeFacil
{
    internal static class Propriedades
    {
        internal static IntercambioTelas Intercambio { get; set; }
        internal static GerenciadorServidor Server { get; set; } = new GerenciadorServidor(new Saida());
    }
}
