using BaseGeral;
using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using Fiscal;
using System;
using System.Collections.Generic;
using Venda;

namespace Comum
{
    public sealed class ControleViewProduto : ControleGenericoViewProdutoFiscal
    {
        NFe Venda { get; set; }
        protected override bool IsNFCe { get; } = false;

        protected override List<DetalhesProdutos> Produtos => Venda.Informacoes.produtos;
        protected override Total Total { set => Venda.Informacoes.total = value; }

        public ControleViewProduto(NFe venda)
        {
            Venda = venda;
        }

        protected override void AbrirTelaDetalhamento(DadosAdicaoProduto dados)
        {
            BasicMainPage.Current.Navegar<ManipulacaoProdutoCompleto>(dados);
        }

        protected override void AbrirTelaEspecifica()
        {
            BasicMainPage.Current.Navegar<ManipulacaoNotaFiscal>(Venda);
        }

        public override void AtualizarControle(object atualizacao)
        {
            if (atualizacao is NFe venda) Venda = venda;
            else throw new InvalidCastException();
        }
    }
}
