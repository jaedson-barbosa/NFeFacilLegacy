using System.Collections.Generic;

namespace NFeFacil.DANFE.Pacotes
{
    public sealed class DadosAdicionais
    {
        public IList<ItemDadosAdicionais> Itens { get; set; }
        
        public DadosAdicionais(IList<ItemDadosAdicionais> itens)
        {
            Itens = itens;
        }
    }
}
