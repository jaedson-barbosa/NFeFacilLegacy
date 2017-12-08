using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static NFeFacil.Extensoes;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto
{
    public sealed class ProdutoOuServico
    {
        [XmlElement(ElementName = "cProd", Order = 0), DescricaoPropriedade("Código")]
        public string CodigoProduto { get; set; }

        [XmlElement(ElementName = "cEAN", Order = 1), DescricaoPropriedade("Código de barras")]
        public string CodigoBarras { get; set; } = "";

        [XmlElement(ElementName = "xProd", Order = 2), DescricaoPropriedade("Descrição")]
        public string Descricao { get; set; }

        [XmlElement(Order = 3)]
        public string NCM { get; set; }

        string cest;
        [XmlElement(ElementName = "CEST", Order = 4), DescricaoPropriedade("Valor total bruto")]
        public string CEST
        {
            get => string.IsNullOrEmpty(cest) ? null : cest;
            set => cest = value;
        }

        [XmlElement("NVE", Order = 5)]
        public string[] NVE { get; set; }

        [XmlElement(Order = 6), DescricaoPropriedade("Preencher de acordo com o código EX da TIPI")]
        public string EXTIPI { get; set; }

        [XmlElement(Order = 7), DescricaoPropriedade("Código Fiscal de Operações e Prestações")]
        public int CFOP { get; set; }

        [XmlElement(ElementName = "uCom", Order = 8), DescricaoPropriedade("Unidade de comercialização")]
        public string UnidadeComercializacao { get; set; }

        double quantidadeComercializada;
        [XmlElement(ElementName = "qCom", Order = 9), DescricaoPropriedade("Quantidade comercializada")]
        public double QuantidadeComercializada
        {
            get => quantidadeComercializada;
            set
            {
                quantidadeComercializada = value;
                DadoImpostoChanged?.Invoke(this, null);
            }
        }

        double valorUnitario;
        [XmlElement(ElementName = "vUnCom", Order = 10), DescricaoPropriedade("Valor unitário de comercialização")]
        public double ValorUnitario
        {
            get => valorUnitario;
            set
            {
                valorUnitario = value;
                DadoImpostoChanged?.Invoke(this, null);
            }
        }

        [XmlIgnore]
        public double ValorTotalDouble
        {
            get => string.IsNullOrEmpty(ValorTotal) ? ValorTotalDouble = 0 : Parse(ValorTotal);
            set
            {
                ValorTotal = ToStr(value);
                DadoImpostoChanged?.Invoke(this, null);
            }
        }
        [XmlElement(ElementName = "vProd", Order = 11), DescricaoPropriedade("Valor total bruto")]
        public string ValorTotal { get; set; }

        [XmlElement(ElementName = "cEANTrib", Order = 12), DescricaoPropriedade("Global Trade Item Number (código de barras do tributo)")]
        public string CodigoBarrasTributo { get; set; } = "";

        [XmlElement(ElementName = "uTrib", Order = 13), DescricaoPropriedade("Unidade de tributação")]
        public string UnidadeTributacao { get; set; }

        double quantidadeTributada;
        [XmlElement(ElementName = "qTrib", Order = 14), DescricaoPropriedade("Quantidade tributada")]
        public double QuantidadeTributada
        {
            get => quantidadeTributada;
            set
            {
                quantidadeTributada = value;
                DadoImpostoChanged?.Invoke(this, null);
            }
        }

        double valorUnitarioTributo;
        [XmlElement(ElementName = "vUnTrib", Order = 15), DescricaoPropriedade("Valor unitário de tributação")]
        public double ValorUnitarioTributo
        {
            get => valorUnitarioTributo;
            set
            {
                valorUnitarioTributo = value;
                DadoImpostoChanged?.Invoke(this, null);
            }
        }

        [XmlElement(ElementName = "vFrete", Order = 16)]
        public string Frete { get; set; }

        [XmlElement(ElementName = "vSeg", Order = 17)]
        public string Seguro { get; set; }

        [XmlElement(ElementName = "vDesc", Order = 18)]
        public string Desconto { get; set; }

        [XmlElement(ElementName = "vOutro", Order = 19), DescricaoPropriedade("Despesas acessórias")]
        public string DespesasAcessorias { get; set; }

        [XmlElement(ElementName = "indTot", Order = 20), DescricaoPropriedade("Inclusão total")]
        public int InclusaoTotal { get; set; } = 1;

        [XmlElement("DI", Order = 21)]
        public List<DeclaracaoImportacao> DI { get; set; } = new List<DeclaracaoImportacao>();

        [XmlElement("detExport", Order = 22)]
        public List<GrupoExportacao> GrupoExportação { get; set; } = new List<GrupoExportacao>();

        [XmlElement(Order = 23), DescricaoPropriedade("Número da Ficha de Conteúdo de Importação")]
        public string NFCI { get; set; }

        [XmlElement(Order = 24), DescricaoPropriedade("Veículo")]
        public VeiculoNovo veicProd { get; set; }

        [XmlElement("med", Order = 25)]
        public List<Medicamento> medicamentos { get; set; }

        [XmlElement("arma", Order = 26)]
        public List<Arma> armas { get; set; }

        [XmlElement(Order = 27), DescricaoPropriedade("Combustível")]
        public Combustivel comb { get; set; }

        [XmlElement("nRECOPI", Order = 28), DescricaoPropriedade("Número RECOPI")]
        public string NRECOPI { get; set; }

        public event EventHandler DadoImpostoChanged;
    }
}
