using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto
{
    public sealed class ProdutoOuServico
    {
        public ProdutoOuServico() { }
        public ProdutoOuServico(BaseProdutoOuServico other)
        {
            CFOP = other.CFOP;
            codigoBarras = other.CodigoBarras;
            codigoBarrasTributo = other.CodigoBarrasTributo;
            codigoProduto = other.CodigoProduto;
            descricao = other.Descricao;
            EXTIPI = other.EXTIPI;
            NCM = other.NCM;
            unidadeComercializacao = other.UnidadeComercializacao;
            unidadeTributacao = other.UnidadeTributacao;
            valorUnitario = other.ValorUnitario;
            valorUnitarioTributo = other.ValorUnitarioTributo;
        }

        /// <summary>
        /// Preencher com CFOP, caso se trate de itens não relacionados com mercadorias/produtos e que o contribuinte não possua codificação própria. Formato: ”CFOP9999”.
        /// </summary>
        [XmlElement(ElementName = "cProd")]
        public string codigoProduto { get; set; }

        /// <summary>
        /// Não informar o conteúdo da TAG em caso de o produto não possuir este código.
        /// </summary>
        [XmlElement(ElementName = "cEAN")]
        public string codigoBarras { get; set; } = "";

        [XmlElement(ElementName = "xProd")]
        public string descricao { get; set; }

        /// <summary>
        /// Obrigatória informação do NCM completo (8 dígitos).
        /// Em caso de item de serviço ou item que não tenham produto (ex. transferência de crédito), informar o valor 00 (dois zeros).
        /// </summary>
        public string NCM { get; set; }

        /// <summary>
        /// (Opcional)
        /// Formato: duas letras maiúsculas e 4 algarismos.
        /// Se a mercadoria se enquadrar em mais de uma codificação, informar até 8 codificações principais.
        /// </summary>
        [XmlElement("NVE")]
        public string[] NVE { get; set; }

        /// <summary>
        /// (Opcional)
        /// Preencher de acordo com o código EX da TIPI. Em caso de serviço, não incluir a TAG.
        /// </summary>
        public string EXTIPI { get; set; }

        /// <summary>
        /// Código Fiscal de Operações e Prestações.
        /// </summary>
        public string CFOP { get; set; }

        /// <summary>
        /// Informar a unidade de comercialização do produto.
        /// </summary>
        [XmlElement(ElementName = "uCom")]
        public string unidadeComercializacao { get; set; }

        /// <summary>
        /// Informar a quantidade de comercialização do produto.
        /// </summary>
        [XmlElement(ElementName = "qCom")]
        public double quantidadeComercializada { get; set; }

        /// <summary>
        /// Informar o valor unitário de comercialização do produto.
        /// </summary>
        [XmlElement(ElementName = "vUnCom")]
        public double valorUnitario { get; set; }

        /// <summary>
        /// Valor Total Bruto dos Produtos ou Serviços.
        /// </summary>
        [XmlElement(ElementName = "vProd")]
        public double valorTotal { get; set; }

        /// <summary>
        /// GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras.
        /// Não informar o conteúdo da TAG em caso de o produto não possuir este código.
        /// </summary>
        [XmlElement(ElementName = "cEANTrib")]
        public string codigoBarrasTributo { get; set; } = "";

        /// <summary>
        /// Unidade Tributável.
        /// </summary>
        [XmlElement(ElementName = "uTrib")]
        public string unidadeTributacao { get; set; }

        /// <summary>
        /// Informar a quantidade de tributação do produto.
        /// </summary>
        [XmlElement(ElementName = "qTrib")]
        public double quantidadeTributada { get; set; }

        /// <summary>
        /// Informar o valor unitário de tributação do produto.
        /// </summary>
        [XmlElement(ElementName = "vUnTrib")]
        public double valorUnitarioTributo { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor Total do Frete.
        /// </summary>
        [XmlElement(ElementName = "vFrete")]
        public string frete { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor Total do Seguro.
        /// </summary>
        [XmlElement(ElementName = "vSeg")]
        public string seguro { get; set; }

        /// <summary>
        /// (Opcional)
        /// Valor do Desconto.
        /// </summary>
        [XmlElement(ElementName = "vDesc")]
        public string desconto { get; set; }

        /// <summary>
        /// (Opcional)
        /// Outras despesas acessórias.
        /// </summary>
        [XmlElement(ElementName = "vOutro")]
        public string despesasAcessórias { get; set; }

        /// <summary>
        /// valor total da NF-e (vProd).
        /// 0=Valor do item(vProd) não compõe o valor total da NF-e;
        /// 1=Valor do item(vProd) compõe o valor total da NF-e.
        /// </summary>
        [XmlElement(ElementName = "indTot")]
        public int inclusãoTotal { get; set; } = 1;

        /// <summary>
        /// (Opcional)
        /// Declaração de Importação.
        /// </summary>
        [XmlElement("DI")]
        public List<DeclaracaoImportacao> DI { get; set; } = new List<DeclaracaoImportacao>();

        /// <summary>
        /// (Opcional)
        /// Grupo de informações de exportação para o item.
        /// </summary>
        [XmlElement("detExport")]
        public List<GrupoExportacao> grupoExportação { get; set; } = new List<GrupoExportacao>();

        /// <summary>
        /// (Opcional)
        /// Número do Pedido de Compra
        /// </summary>
        public string xPed { get; set; }

        /// <summary>
        /// (Opcional)
        /// Número do item do pedido de compra, o campo é de livre uso do emissor.
        /// </summary>
        public string nItemPed { get; set; }

        /// <summary>
        /// (Opcional)
        /// Número de controle da FCI - Ficha de Conteúdo de Importação.
        /// Ex.: B01F70AF-10BF-4B1F-848C-65FF57F616FE
        /// </summary>
        public string nFCI { get; set; }

        public VeiculoNovo veicProd;

        [XmlElement("med")]
        public List<Medicamento> medicamentos = new List<Medicamento>();

        [XmlElement("arma")]
        public List<Arma> armas = new List<Arma>();

        public Combustivel comb;

        public string nRECOPI { get; set; }
    }
}
