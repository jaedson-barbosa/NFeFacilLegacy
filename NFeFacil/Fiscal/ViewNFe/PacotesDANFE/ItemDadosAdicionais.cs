using System.Collections.Generic;

namespace NFeFacil.Fiscal.ViewNFe.PacotesDANFE
{
    public struct ItemDadosAdicionais
    {
        public string Titulo { get; set; }
        public IEnumerable<string> Linhas { get; set; }

        public ItemDadosAdicionais(string titulo, IEnumerable<string> linhas)
        {
            Titulo = titulo;
            Linhas = linhas;
        }

        public ItemDadosAdicionais(string titulo, params string[] linhas)
        {
            Titulo = titulo;
            Linhas = linhas;
        }
    }
}
