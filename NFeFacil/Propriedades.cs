using NFeFacil.Log;
using NFeFacil.NavegacaoUI;
using NFeFacil.Sincronizacao;

namespace NFeFacil
{
    internal static class Propriedades
    {
        internal static IntercambioTelas Intercambio { get; set; }
        internal static GerenciadorServidor Server { get; set; } = new GerenciadorServidor(new Saida());
    }
}
