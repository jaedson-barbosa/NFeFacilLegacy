using BaseGeral;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using System.Collections.Generic;
using Venda;

namespace Consumidor
{
    public sealed class ControleViewProduto : ControleGenericoViewProdutoFiscal
    {
        NFCe Venda { get; }

        protected override List<DetalhesProdutos> Produtos => Venda.Informacoes.produtos;
        protected override Total Total { set => Venda.Informacoes.total = value; }

        public ControleViewProduto(NFCe venda)
        {
            Venda = venda;
        }

        protected override void AbrirTelaDetalhamento(DadosAdicaoProduto dados)
        {
            BasicMainPage.Current.Navegar<ProdutoNFCe>(dados);
        }

        protected override void AbrirTelaEspecifica()
        {
            BasicMainPage.Current.Navegar<ManipulacaoNFCe>(Venda);
        }
    }
}
