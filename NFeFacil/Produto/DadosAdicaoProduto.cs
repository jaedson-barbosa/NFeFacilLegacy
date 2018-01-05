using System.Collections.Generic;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;

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
        }

        public DetalhesProdutos Completo { get; }
        public ProdutoDI Auxiliar { get; }

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
