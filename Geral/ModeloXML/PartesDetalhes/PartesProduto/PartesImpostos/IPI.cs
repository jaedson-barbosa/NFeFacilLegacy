using BaseGeral.View;
using System.Xml.Serialization;
using static BaseGeral.ExtensoesPrincipal;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Imposto sobre Produtos Industrializados.
    /// </summary>
    public class IPI : IImposto
    {
        [XmlElement(Order = 0), DescricaoPropriedade("CNPJ do produtor da mercadoria, quando diferente do emitente")]
        public string CNPJProd { get; set; }

        [XmlElement(Order = 1), DescricaoPropriedade("Código do selo de controle IPI")]
        public string CodigoSelo { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Quantidade de selos de controle")]
        public string QuantidadeSelos { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Código de Enquadramento Legal do IPI")]
        public string CodigoEnquadramento { get; set; }

        [XmlElement(nameof(IPINT), Type = typeof(IPINT), Order = 4),
            XmlElement(nameof(IPITrib), Type = typeof(IPITrib), Order = 4)]
        public ComumIPI Corpo { get; set; }
    }

    public abstract class ComumIPI
    {
        [DescricaoPropriedade("Código da situação tributária do IPI")]
        [XmlElement(Order = 0)]
        public string CST { get; set; }

        protected ComumIPI(string cst)
        {
            if (!string.IsNullOrEmpty(cst))
                CST = cst;
        }
    }

    /// <summary>
    /// Grupo CST 01, 02, 03, 04, 51, 52, 53, 54 e 55.
    /// </summary>
    public sealed class IPINT : ComumIPI
    {
        [System.Obsolete]
        public IPINT() : base(null) { }
        public IPINT(string cst) : base(cst) { }
    }

    /// <summary>
    /// Grupo do CST 00, 49, 50 e 99.
    /// </summary>
    public sealed class IPITrib : ComumIPI
    {
        [XmlElement(Order = 1), DescricaoPropriedade("Valor da BC do IPI")]
        public string ValorBC { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Alíquota do IPI")]
        public string AliquotaPercentual { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Quantidade total na unidade padrão para tributação")]
        public string Quantidade { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Valor por Unidade Tributável")]
        public string AliquotaReais { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Valor do IPI")]
        public string ValorIPI { get; set; }

        [System.Obsolete]
        public IPITrib() : base(null) { }
        public IPITrib(string cst, double bcOuQuant, double aliquota, bool calcPelaQuantidade) : base(cst)
        {
            if (calcPelaQuantidade)
            {
                Quantidade = ToStr(bcOuQuant, "F4");
                AliquotaReais = ToStr(aliquota, "F4");
                ValorIPI = ToStr(bcOuQuant * aliquota);
            }
            else
            {
                ValorBC = ToStr(bcOuQuant);
                AliquotaPercentual = ToStr(aliquota, "F4");
                ValorIPI = ToStr(bcOuQuant * aliquota / 100);
            }
        }
    }
}
