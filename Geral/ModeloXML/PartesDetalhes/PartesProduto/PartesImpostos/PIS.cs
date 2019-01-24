using BaseGeral.View;
using System.Xml.Serialization;
using static BaseGeral.ExtensoesPrincipal;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class PIS : IImposto
    {
        [XmlElement(nameof(PISAliq), Type = typeof(PISAliq), Order = 0),
            XmlElement(nameof(PISNT), Type = typeof(PISNT), Order = 0),
            XmlElement(nameof(PISOutr), Type = typeof(PISOutr), Order = 0),
            XmlElement(nameof(PISQtde), Type = typeof(PISQtde), Order = 0),
            DescricaoPropriedade("Corpo do PIS")]
        public ComumPIS Corpo { get; set; }
    }

    public abstract class ComumPIS
    {
        [DescricaoPropriedade("Código de Situação Tributária da PIS")]
        [XmlElement(Order = 0)]
        public string CST { get; set; }

        [DescricaoPropriedade("Valor da Base de Cálculo da PIS")]
        [XmlElement("vBC", Order = 1)]
        public string ValorBC { get; set; }

        [DescricaoPropriedade("Alíquota da PIS (em percentual)")]
        [XmlElement("pPIS", Order = 2)]
        public string AliquotaPercentual { get; set; }

        [DescricaoPropriedade("Quantidade Vendida")]
        [XmlElement("qBCProd", Order = 3)]
        public string Quantidade { get; set; }

        [DescricaoPropriedade("Alíquota da PIS (em reais)")]
        [XmlElement("vAliqProd", Order = 4)]
        public string AliquotaReais { get; set; }

        [DescricaoPropriedade("Valor da PIS")]
        [XmlElement("vPIS", Order = 5)]
        public string ValorPIS { get; set; }

        protected ComumPIS(string cst)
        {
            if (!string.IsNullOrEmpty(cst))
                CST = cst;
        }

        protected ComumPIS(string cst, double bcOuQuant, double aliquota, bool calcPelaQuantidade) : this(cst)
        {
            if (calcPelaQuantidade)
            {
                Quantidade = ToStr(bcOuQuant, "F4");
                AliquotaReais = ToStr(aliquota, "F4");
                ValorPIS = ToStr(bcOuQuant * aliquota);
            }
            else
            {
                ValorBC = ToStr(bcOuQuant);
                AliquotaPercentual = ToStr(aliquota, "F4");
                ValorPIS = ToStr(bcOuQuant * aliquota / 100);
            }
        }
    }

    /// <summary>
    /// Grupo PIS tributado pela alíquota.
    /// </summary>
    public sealed class PISAliq : ComumPIS
    {
        [System.Obsolete]
        public PISAliq() : base(null) { }
        public PISAliq(string cst, double vBC, double pPIS) : base(cst, vBC, pPIS, false) { }
    }

    /// <summary>
    /// Grupo PIS não tributado.
    /// </summary>
    public sealed class PISNT : ComumPIS
    {
        [System.Obsolete]
        public PISNT() : base(null) { }
        public PISNT(string cst) : base(cst) { }
    }

    /// <summary>
    /// Grupo PIS Outras Operações.
    /// </summary>
    public sealed class PISOutr : ComumPIS
    {
        [System.Obsolete]
        public PISOutr() : base(null) { }
        public PISOutr(string cst, double bcOuQuant, double aliquota, bool calcPelaQuantidade)
            : base(cst, bcOuQuant, aliquota, calcPelaQuantidade) { }
    }

    /// <summary>
    /// Grupo de PIS tributado por quantidade
    /// </summary>
    public sealed class PISQtde : ComumPIS
    {
        [System.Obsolete]
        public PISQtde() : base(null) { }
        public PISQtde(string cst, double qBCProd, double vAliqProd) : base(cst, qBCProd, vAliqProd, true) { }
    }

    /// <summary>
    /// Grupo PIS Substituição Tributária.
    /// Só deve ser informado se o Produto for sujeito a PIS por ST (CST = 05).
    /// </summary>
    public class PISST : ComumPIS, IImposto
    {
        [System.Obsolete]
        public PISST() : base(null) { }
        public PISST(double bcOuQuant, double aliquota, bool calcPelaQuantidade)
            : base(null, bcOuQuant, aliquota, calcPelaQuantidade) { }
    }
}
