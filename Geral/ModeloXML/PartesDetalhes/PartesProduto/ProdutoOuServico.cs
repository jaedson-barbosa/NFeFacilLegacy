using BaseGeral.View;
using System.Xml.Serialization;
using static BaseGeral.ExtensoesPrincipal;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto
{
    public sealed class ProdutoOuServico
    {
        [XmlElement(ElementName = "cProd", Order = 0), DescricaoPropriedade("Código")]
        public string CodigoProduto { get; set; }

        string codigoBarras;
        [XmlElement(ElementName = "cEAN", Order = 1), DescricaoPropriedade("Código de barras")]
        public string CodigoBarras
        {
            get => string.IsNullOrEmpty(codigoBarras) ? "SEM GTIN" : codigoBarras;
            set => codigoBarras = value;
        }

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

        [XmlIgnore]
        public double QuantidadeComercializada
        {
            get => string.IsNullOrEmpty(QuantidadeComercializadaString) ? ValorTotal = 0 : Parse(QuantidadeComercializadaString);
            set => QuantidadeComercializadaString = ToStr(value);
        }

        [XmlElement(ElementName = "qCom", Order = 9), DescricaoPropriedade("Quantidade comercializada")]
        public string QuantidadeComercializadaString { get; set; }

        [XmlIgnore]
        public double ValorUnitario
        {
            get => string.IsNullOrEmpty(ValorUnitarioString) ? ValorTotal = 0 : Parse(ValorUnitarioString);
            set => ValorUnitarioString = ToStr(value);
        }

        [XmlElement(ElementName = "vUnCom", Order = 10), DescricaoPropriedade("Valor unitário de comercialização")]
        public string ValorUnitarioString { get; set; }

        [XmlIgnore]
        public double ValorTotal
        {
            get => string.IsNullOrEmpty(ValorTotalString) ? ValorTotal = 0 : Parse(ValorTotalString);
            set => ValorTotalString = ToStr(value);
        }

        [XmlElement(ElementName = "vProd", Order = 11), DescricaoPropriedade("Valor total bruto")]
        public string ValorTotalString { get; set; }

        string codigoBarrasTributo;
        [XmlElement(ElementName = "cEANTrib", Order = 12), DescricaoPropriedade("Global Trade Item Number (código de barras do tributo)")]
        public string CodigoBarrasTributo
        {
            get => string.IsNullOrEmpty(codigoBarrasTributo) ? "SEM GTIN" : codigoBarrasTributo;
            set => codigoBarrasTributo = value;
        }

        [XmlElement(ElementName = "uTrib", Order = 13), DescricaoPropriedade("Unidade de tributação")]
        public string UnidadeTributacao { get; set; }

        [XmlIgnore]
        public double QuantidadeTributada
        {
            get => string.IsNullOrEmpty(QuantidadeTributadaString) ? ValorTotal = 0 : Parse(QuantidadeTributadaString);
            set => QuantidadeTributadaString = ToStr(value);
        }

        [XmlElement(ElementName = "qTrib", Order = 14), DescricaoPropriedade("Quantidade tributada")]
        public string QuantidadeTributadaString { get; set; }

        [XmlIgnore]
        public double ValorUnitarioTributo
        {
            get => string.IsNullOrEmpty(ValorUnitarioTributoString) ? ValorTotal = 0 : Parse(ValorUnitarioTributoString);
            set => ValorUnitarioTributoString = ToStr(value);
        }

        [XmlElement(ElementName = "vUnTrib", Order = 15), DescricaoPropriedade("Valor unitário de tributação")]
        public string ValorUnitarioTributoString { get; set; }

        [XmlIgnore]
        public double Frete
        {
            get => string.IsNullOrEmpty(FreteString) ? 0 : Parse(FreteString);
            set => FreteString = value != 0 ? ToStr(value) : null;
        }

        [XmlElement(ElementName = "vFrete", Order = 16)]
        public string FreteString { get; set; }

        [XmlIgnore]
        public double Seguro
        {
            get => string.IsNullOrEmpty(SeguroString) ? 0 : Parse(SeguroString);
            set => SeguroString = value != 0 ? ToStr(value) : null;
        }

        [XmlElement(ElementName = "vSeg", Order = 17)]
        public string SeguroString { get; set; }

        [XmlIgnore]
        public double Desconto
        {
            get => string.IsNullOrEmpty(DescontoString) ? 0 : Parse(DescontoString);
            set => DescontoString = value != 0 ? ToStr(value) : null;
        }

        [XmlElement(ElementName = "vDesc", Order = 18)]
        public string DescontoString { get; set; }

        [XmlIgnore]
        public double DespesasAcessorias
        {
            get => string.IsNullOrEmpty(DespesasAcessoriasString) ? 0 : Parse(DespesasAcessoriasString);
            set => DespesasAcessoriasString = value != 0 ? ToStr(value) : null;
        }

        [XmlElement(ElementName = "vOutro", Order = 19), DescricaoPropriedade("Despesas acessórias")]
        public string DespesasAcessoriasString { get; set; }

        [XmlElement(ElementName = "indTot", Order = 20), DescricaoPropriedade("Inclusão total")]
        public int InclusaoTotal { get; set; } = 1;

        [XmlElement(Order = 23), DescricaoPropriedade("Número da Ficha de Conteúdo de Importação")]
        public string NFCI { get; set; }

        [XmlElement(Order = 26), DescricaoPropriedade("Combustível")]
        public Combustivel Combustivel { get; set; }
    }
}
