using System.Collections.Generic;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using NFeFacil.Produto.Impostos;

namespace NFeFacil.Produto
{
    class DadosAdicaoProduto : IProdutoEspecial
    {
        public DadosAdicaoProduto(ProdutoDI auxiliar)
        {
            Completo = new DetalhesProdutos
            {
                Produto = auxiliar.ToProdutoOuServico()
            };
            Auxiliar = auxiliar;
            ImpostosPadrao = auxiliar.GetImpostosPadrao();
        }

        public DadosAdicaoProduto(ProdutoDI auxiliar, DetalhesProdutos completo)
        {
            Completo = completo;
            Auxiliar = auxiliar;
            ImpostosPadrao = auxiliar.GetImpostosPadrao();
        }

        public DetalhesProdutos Completo { get; }
        public ProdutoDI Auxiliar { get; }
        public (PrincipaisImpostos Tipo, string NomeTemplate, int CST)[] ImpostosPadrao { get; }

        List<Arma> IProdutoEspecial.armas
        {
            get => ((IProdutoEspecial)Completo).armas;
            set => ((IProdutoEspecial)Completo).armas = value;
        }
        Combustivel IProdutoEspecial.comb
        {
            get => ((IProdutoEspecial)Completo).comb;
            set => ((IProdutoEspecial)Completo).comb = value;
        }
        List<Medicamento> IProdutoEspecial.medicamentos
        {
            get => ((IProdutoEspecial)Completo).medicamentos;
            set => ((IProdutoEspecial)Completo).medicamentos = value;
        }
        string IProdutoEspecial.NRECOPI
        {
            get => ((IProdutoEspecial)Completo).NRECOPI;
            set => ((IProdutoEspecial)Completo).NRECOPI = value;
        }
        VeiculoNovo IProdutoEspecial.veicProd
        {
            get => ((IProdutoEspecial)Completo).veicProd;
            set => ((IProdutoEspecial)Completo).veicProd = value;
        }
    }
}
