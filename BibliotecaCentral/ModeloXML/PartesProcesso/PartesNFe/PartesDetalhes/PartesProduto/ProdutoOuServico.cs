using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto
{
    public sealed class ProdutoOuServico
    {
        public ProdutoOuServico() { }
        public ProdutoOuServico(BaseProdutoOuServico other)
        {
            CFOP = other.CFOP;
            CodigoBarras = other.CodigoBarras;
            CodigoBarrasTributo = other.CodigoBarrasTributo;
            CodigoProduto = other.CodigoProduto;
            Descricao = other.Descricao;
            EXTIPI = other.EXTIPI;
            NCM = other.NCM;
            UnidadeComercializacao = other.UnidadeComercializacao;
            UnidadeTributacao = other.UnidadeTributacao;
            ValorUnitario = other.ValorUnitario;
            ValorUnitarioTributo = other.ValorUnitarioTributo;
        }

        /// <summary>
        /// Preencher com CFOP, caso se trate de itens não relacionados com mercadorias/produtos e que o contribuinte não possua codificação própria. Formato: ”CFOP9999”.
        /// </summary>
        [XmlElement(ElementName = "cProd", Order = 0)]
        public string CodigoProduto { get; set; }

        /// <summary>
        /// Não informar o conteúdo da TAG em caso de o Produto não possuir este código.
        /// </summary>
        [XmlElement(ElementName = "cEAN", Order = 1)]
        public string CodigoBarras { get; set; } = "";

        [XmlElement(ElementName = "xProd", Order = 2)]
        public string Descricao { get; set; }

        /// <summary>
        /// Obrigatória informação do NCM completo (8 dígitos).
        /// Em caso de item de serviço ou item que não tenham Produto (ex. transferência de crédito), informar o valor 00 (dois zeros).
        /// </summary>
        [XmlElement(Order = 3)]
        public string NCM { get; set; }

        /// <summary>
        /// (Opcional)
        /// Formato: duas letras maiúsculas e 4 algarismos.
        /// Se a mercadoria se enquadrar em mais de uma codificação, informar até 8 codificações principais.
        /// </summary>
        [XmlElement("NVE", Order = 4)]
        public string[] NVE { get; set; }

        /// <summary>
        /// (Opcional)
        /// Preencher de acordo com o código EX da TIPI. Em caso de serviço, não incluir a TAG.
        /// </summary>
        [XmlElement(Order = 5)]
        public string EXTIPI { get; set; }

        /// <summary>
        /// Código Fiscal de Operações e Prestações.
        /// </summary>
        [XmlElement(Order = 6)]
        public string CFOP { get; set; }

        /// <summary>
        /// Informar a unidade de comercialização do Produto.
        /// </summary>
        [XmlElement(ElementName = "uCom", Order = 7)]
        public string UnidadeComercializacao { get; set; }

        /// <summary>
        /// Informar a quantidade de comercialização do Produto.
        /// </summary>
        [XmlElement(ElementName = "qCom", Order = 8)]
        public double QuantidadeComercializada { get; set; }

        /// <summary>
        /// Informar o valor unitário de comercialização do Produto.
        /// </summary>
        [XmlElement(ElementName = "vUnCom", Order = 9)]
        public double ValorUnitario { get; set; }

        /// <summary>
        /// Valor Total Bruto dos Produtos ou Serviços.
        /// </summary>
        [XmlElement(ElementName = "vProd", Order = 10)]
        public double ValorTotal { get; set; }

        /// <summary>
        /// GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras.
        /// Não informar o conteúdo da TAG em caso de o Produto não possuir este código.
        /// </summary>
        [XmlElement(ElementName = "cEANTrib", Order = 11)]
        public string CodigoBarrasTributo { get; set; } = "";

        /// <summary>
        /// Unidade Tributável.
        /// </summary>
        [XmlElement(ElementName = "uTrib", Order = 12)]
        public string UnidadeTributacao { get; set; }

        /// <summary>
        /// Informar a quantidade de tributação do Produto.
        /// </summary>
        [XmlElement(ElementName = "qTrib", Order = 13)]
        public double QuantidadeTributada { get; set; }

        /// <summary>
        /// Informar o valor unitário de tributação do Produto.
        /// </summary>
        [XmlElement(ElementName = "vUnTrib", Order = 14)]
        public double ValorUnitarioTributo { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor Total do Frete.
        /// </summary>
        [XmlElement(ElementName = "vFrete", Order = 15)]
        public string Frete { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor Total do Seguro.
        /// </summary>
        [XmlElement(ElementName = "vSeg", Order = 16)]
        public string Seguro { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor do Desconto.
        /// </summary>
        [XmlElement(ElementName = "vDesc", Order = 17)]
        public string Desconto { get; set; }

        /// <summary>
        /// (Opcional)
        /// Outras despesas acessórias.
        /// </summary>
        [XmlElement(ElementName = "vOutro", Order = 18)]
        public string DespesasAcessórias { get; set; }

        /// <summary>
        /// valor total da NF-e (vProd).
        /// 0=Valor do item(vProd) não compõe o valor total da NF-e;
        /// 1=Valor do item(vProd) compõe o valor total da NF-e.
        /// </summary>
        [XmlElement(ElementName = "indTot", Order = 19)]
        public int InclusãoTotal { get; set; } = 1;

        /// <summary>
        /// (Opcional)
        /// Declaração de Importação.
        /// </summary>
        [XmlElement("DI", Order = 20)]
        public List<DeclaracaoImportacao> DI { get; set; } = new List<DeclaracaoImportacao>();

        /// <summary>
        /// (Opcional)
        /// Grupo de informações de exportação para o item.
        /// </summary>
        [XmlElement("detExport", Order = 21)]
        public List<GrupoExportacao> GrupoExportação { get; set; } = new List<GrupoExportacao>();

        /// <summary>
        /// (Opcional)
        /// Número do Pedido de Compra
        /// </summary>
        [XmlElement(Order = 22)]
        public string XPed { get; set; }

        /// <summary>
        /// (Opcional)
        /// Número do item do pedido de compra, o campo é de livre uso do emissor.
        /// </summary>
        [XmlElement(Order = 23)]
        public string NItemPed { get; set; }

        /// <summary>
        /// (Opcional)
        /// Número de controle da FCI - Ficha de Conteúdo de Importação.
        /// Ex.: B01F70AF-10BF-4B1F-848C-65FF57F616FE
        /// </summary>
        [XmlElement(Order = 24)]
        public string NFCI { get; set; }

        [XmlElement(Order = 25)]
        public VeiculoNovo veicProd;

        [XmlElement("med", Order = 26)]
        public List<Medicamento> medicamentos = new List<Medicamento>();

        [XmlElement("arma", Order = 27)]
        public List<Arma> armas = new List<Arma>();

        [XmlElement(Order = 28)]
        public Combustivel comb;

        [XmlElement("nRECOPI", Order = 29)]
        public string NRECOPI { get; set; }
    }
}
