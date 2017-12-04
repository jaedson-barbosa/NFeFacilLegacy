using System.Collections.Generic;

namespace NFeFacil.DANFE.Pacotes
{
    public sealed class DadosAdicionais
    {
        public List<ItemDadosAdicionais> Itens { get; set; }
        
        public DadosAdicionais(List<ItemDadosAdicionais> itens)
        {
            Itens = itens;
        }
    }
}
