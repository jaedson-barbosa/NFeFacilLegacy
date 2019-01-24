using BaseGeral.View;
using System.Xml.Serialization;
using static BaseGeral.ExtensoesPrincipal;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class COFINS : IImposto
    {
        [XmlElement(nameof(COFINSAliq), Type = typeof(COFINSAliq), Order = 0),
            XmlElement(nameof(COFINSNT), Type = typeof(COFINSNT), Order = 0),
            XmlElement(nameof(COFINSOutr), Type = typeof(COFINSOutr), Order = 0),
            XmlElement(nameof(COFINSQtde), Type = typeof(COFINSQtde), Order = 0),
            DescricaoPropriedade("Corpo do COFINS")]
        public ComumCOFINS Corpo { get; set; }
    }

    public abstract class ComumCOFINS
    {
        [DescricaoPropriedade("Código de Situação Tributária da COFINS")]
        [XmlElement(Order = 0)]
        public string CST { get; set; }

        [DescricaoPropriedade("Valor da Base de Cálculo da COFINS")]
        [XmlElement("vBC", Order = 1)]
        public string ValorBC { get; set; }

        [DescricaoPropriedade("Alíquota da COFINS (em percentual)")]
        [XmlElement("pCOFINS", Order = 2)]
        public string AliquotaPercentual { get; set; }

        [DescricaoPropriedade("Quantidade Vendida")]
        [XmlElement("qBCProd", Order = 3)]
        public string Quantidade { get; set; }

        [DescricaoPropriedade("Alíquota da COFINS (em reais)")]
        [XmlElement("vAliqProd", Order = 4)]
        public string AliquotaReais { get; set; }

        [DescricaoPropriedade("Valor da COFINS")]
        [XmlElement("vCOFINS", Order = 5)]
        public string ValorCOFINS { get; set; }

        protected ComumCOFINS(string cst)
        {
            if (!string.IsNullOrEmpty(cst))
                CST = cst;
        }

        protected ComumCOFINS(string cst, double bcOuQuant, double aliquota, bool calcPelaQuantidade) : this(cst)
        {
            if (calcPelaQuantidade)
            {
                Quantidade = ToStr(bcOuQuant, "F4");
                AliquotaReais = ToStr(aliquota, "F4");
                ValorCOFINS = ToStr(bcOuQuant * aliquota);
            }
            else
            {
                ValorBC = ToStr(bcOuQuant);
                AliquotaPercentual = ToStr(aliquota, "F4");
                ValorCOFINS = ToStr(bcOuQuant * aliquota / 100);
            }
        }
    }

    /// <summary>
    /// Grupo COFINS tributado pela alíquota.
    /// </summary>
    public sealed class COFINSAliq : ComumCOFINS
    {
        [System.Obsolete]
        public COFINSAliq() : base(null) { }
        public COFINSAliq(string cst, double vBC, double pCOFINS) : base(cst, vBC, pCOFINS, false) { }
    }

    /// <summary>
    /// Grupo COFINS não tributado.
    /// </summary>
    public sealed class COFINSNT : ComumCOFINS
    {
        [System.Obsolete]
        public COFINSNT() : base(null) { }
        public COFINSNT(string cst) : base(cst) { }
    }

    /// <summary>
    /// Grupo COFINS Outras Operações.
    /// </summary>
    public sealed class COFINSOutr : ComumCOFINS
    {
        [System.Obsolete]
        public COFINSOutr() : base(null) { }
        public COFINSOutr(string cst, double bcOuQuant, double aliquota, bool calcPelaQuantidade)
            : base(cst, bcOuQuant, aliquota, calcPelaQuantidade) { }
    }

    /// <summary>
    /// Grupo de COFINS tributado por quantidade
    /// </summary>
    public sealed class COFINSQtde : ComumCOFINS
    {
        [System.Obsolete]
        public COFINSQtde() : base(null) { }
        public COFINSQtde(string cst, double qBCProd, double vAliqProd) : base(cst, qBCProd, vAliqProd, true) { }
    }

    /// <summary>
    /// Grupo COFINS Substituição Tributária.
    /// Só deve ser informado se o Produto for sujeito a COFINS por ST (CST = 05).
    /// </summary>
    public class COFINSST : ComumCOFINS, IImposto
    {
        [System.Obsolete]
        public COFINSST() : base(null) { }
        public COFINSST(double bcOuQuant, double aliquota, bool calcPelaQuantidade)
            : base(null, bcOuQuant, aliquota, calcPelaQuantidade) { }
    }
}
