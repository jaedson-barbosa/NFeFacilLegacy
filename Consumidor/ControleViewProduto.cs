using BaseGeral;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using Fiscal;
using System;
using System.Collections.Generic;
using Venda;

namespace Consumidor
{
    public sealed class ControleViewProduto : ControleGenericoViewProdutoFiscal
    {
        NFCe Venda { get; set; }
        protected override bool IsNFCe { get; } = true;

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

        public override void AtualizarControle(object atualizacao)
        {
            if (atualizacao is NFCe venda) Venda = venda;
            else throw new InvalidCastException();
        }
    }
}
