using System.Collections.Generic;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Produtos : ConexaoBanco
    {
        public IEnumerable<BaseProdutoOuServico> Registro => Contexto.Produtos;
        public void Adicionar(BaseProdutoOuServico dado) => Contexto.Add(dado);
        public void Atualizar(BaseProdutoOuServico dado) => Contexto.Update(dado);
        public void Remover(BaseProdutoOuServico dado) => Contexto.Remove(dado);
    }
}
