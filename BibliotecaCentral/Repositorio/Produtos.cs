using System.Collections.Generic;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Produtos : ConexaoBanco
    {
        public IEnumerable<BaseProdutoOuServico> Registro => Contexto.Produtos;

        public void Adicionar(BaseProdutoOuServico dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Add(dado);
        }

        public void Atualizar(BaseProdutoOuServico dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Update(dado);
        }

        public void Remover(BaseProdutoOuServico dado)
        {
            Contexto.Remove(dado);
        }
    }
}
