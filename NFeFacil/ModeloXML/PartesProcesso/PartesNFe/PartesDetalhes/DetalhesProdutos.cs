using NFeFacil.AtributosVisualizacao;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    [XmlRoot(ElementName = "det", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class DetalhesProdutos : IProdutoEspecial
    {
        [XmlAttribute(AttributeName = "nItem")]
        public int Número { get; set; }

        [XmlElement("prod", Order = 0)]
        public ProdutoOuServico Produto { get; set; } = new ProdutoOuServico();

        [XmlElement("imposto", Order = 1)]
        public Impostos Impostos { get; set; } = new Impostos();

        [XmlElement("impostoDevol", Order = 2), DescricaoPropriedade("Imposto devolvido")]
        public ImpostoDevol ImpostoDevol { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informações Adicionais do DadosProduto.
        /// </summary>
        [XmlElement("infAdProd", Order = 3), DescricaoPropriedade("Informações adicionais")]
        public string InfAdProd { get; set; }

        List<Arma> IProdutoEspecial.armas { get => Produto.armas; set => Produto.armas = value; }
        Combustivel IProdutoEspecial.comb { get => Produto.comb; set => Produto.comb = value; }
        List<Medicamento> IProdutoEspecial.medicamentos { get => Produto.medicamentos; set => Produto.medicamentos = value; }
        string IProdutoEspecial.NRECOPI { get => Produto.NRECOPI; set => Produto.NRECOPI = value; }
        VeiculoNovo IProdutoEspecial.veicProd { get => Produto.veicProd; set => Produto.veicProd = value; }
    }
}
