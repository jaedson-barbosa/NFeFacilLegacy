using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BaseGeral.ItensBD
{
    public class ProdutoDI : IStatusAtivacao, IGuidId, IProdutoEspecial
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public string CodigoProduto { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public string Descricao { get; set; }
        public string NCM { get; set; }
        public string EXTIPI { get; set; }
        public string CFOP { get; set; }
        public string UnidadeComercializacao { get; set; }
        public double ValorUnitario { get; set; }
        public string CodigoBarrasTributo { get; set; } = string.Empty;
        public string UnidadeTributacao { get; set; }
        public double ValorUnitarioTributo { get; set; }

        public bool Ativo { get; set; } = true;
        public string ProdutoEspecial { get; set; }
        public string CEST { get; set; }

        public Guid IdFornecedor { get; set; }
        public Guid IdCategoria { get; set; }

        Combustivel detalheEspecial;
        Combustivel IProdutoEspecial.comb
        {
            get
            {
                if (detalheEspecial == null)
                {
                    if (string.IsNullOrEmpty(ProdutoEspecial))
                        detalheEspecial = null;
                    else
                    {
                        var xml = XElement.Parse(ProdutoEspecial);
                        var tipo = (TiposProduto)Enum.Parse(typeof(TiposProduto), xml.Name.LocalName);
                        if (tipo == TiposProduto.Combustivel)
                            detalheEspecial = xml.FirstNode.FromXElement<Combustivel>();
                    }
                }
                return detalheEspecial;
            }
            set
            {
                if (value != null)
                {
                    detalheEspecial = value;
                    ProdutoEspecial = new XElement(TiposProduto.Combustivel.ToString(),
                        value.ToXElement<Combustivel>()).ToString(SaveOptions.DisableFormatting);
                }
            }
        }

        public string ImpostosSimples { get; set; }
        public string ICMS { get; set; }

        public string ImpostosPadrao { get; set; }

        public ProdutoDI() { }
        public ProdutoDI(ProdutoOuServicoGenerico other)
        {
            CodigoProduto = other.CodigoProduto;
            CodigoBarras = other.CodigoBarras;
            Descricao = other.Descricao;
            NCM = other.NCM;
            EXTIPI = other.EXTIPI;
            CFOP = other.CFOP.ToString();
            UnidadeComercializacao = other.UnidadeComercializacao;
            ValorUnitario = other.ValorUnitario;
            CodigoBarrasTributo = other.CodigoBarrasTributo;
            UnidadeTributacao = other.UnidadeTributacao;
            ValorUnitarioTributo = other.ValorUnitarioTributo;
        }

        public ProdutoOuServico ToProdutoOuServico()
        {
            var especial = (IProdutoEspecial)this;
            return new ProdutoOuServico
            {
                CodigoProduto = CodigoProduto,
                CodigoBarras = CodigoBarras,
                Descricao = Descricao,
                NCM = NCM,
                EXTIPI = string.IsNullOrEmpty(EXTIPI) ? null : EXTIPI,
                CFOP = string.IsNullOrEmpty(CFOP) ? 0 : int.Parse(CFOP),
                UnidadeComercializacao = UnidadeComercializacao,
                ValorUnitario = ValorUnitario,
                CodigoBarrasTributo = CodigoBarrasTributo,
                UnidadeTributacao = UnidadeTributacao,
                ValorUnitarioTributo = ValorUnitarioTributo,
                CEST = string.IsNullOrEmpty(CEST) ? null : CEST,
                comb = especial.comb,
            };
        }

        public void ResetEspecial()
        {
            detalheEspecial = null;
            ProdutoEspecial = null;
        }

        public sealed class ProdutoOuServicoGenerico
        {
            [XmlElement(ElementName = "cProd")]
            public string CodigoProduto { get; set; }

            [XmlElement(ElementName = "cEAN")]
            public string CodigoBarras { get; set; } = "";

            [XmlElement(ElementName = "xProd")]
            public string Descricao { get; set; }

            public string NCM { get; set; }

            public string EXTIPI { get; set; }

            public int CFOP { get; set; }

            [XmlElement(ElementName = "uCom")]
            public string UnidadeComercializacao { get; set; }

            [XmlElement(ElementName = "vUnCom")]
            public double ValorUnitario { get; set; }

            [XmlElement(ElementName = "cEANTrib")]
            public string CodigoBarrasTributo { get; set; } = "";

            [XmlElement(ElementName = "uTrib")]
            public string UnidadeTributacao { get; set; }

            [XmlElement(ElementName = "vUnTrib")]
            public double ValorUnitarioTributo { get; set; }
        }

        public enum TiposProduto { Simples, Combustivel }
    }
}
