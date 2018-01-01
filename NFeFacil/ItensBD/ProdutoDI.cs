using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using NFeFacil.Produto;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ItensBD
{
    public sealed class ProdutoDI : IStatusAtivacao, IGuidId, IProdutoEspecial
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

        object detalheEspecial;
        object DetalheEspecial
        {
            get
            {
                if (detalheEspecial == null)
                {
                    if (string.IsNullOrEmpty(ProdutoEspecial))
                    {
                        detalheEspecial = null;
                    }
                    else
                    {
                        var xml = XElement.Parse(ProdutoEspecial);
                        var tipo = (TiposProduto)Enum.Parse(typeof(TiposProduto), xml.Name.LocalName);
                        switch (tipo)
                        {
                            case TiposProduto.Veiculo:
                                detalheEspecial = xml.FirstNode.FromXElement<VeiculoNovo>();
                                break;
                            case TiposProduto.Medicamento:
                                detalheEspecial = xml.FirstNode.FromXElement<List<Medicamento>>();
                                break;
                            case TiposProduto.Armamento:
                                detalheEspecial = xml.FirstNode.FromXElement<List<Arma>>();
                                break;
                            case TiposProduto.Combustivel:
                                detalheEspecial = xml.FirstNode.FromXElement<Combustivel>();
                                break;
                            case TiposProduto.Papel:
                                detalheEspecial = xml.Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
                return detalheEspecial;
            }
            set => detalheEspecial = value;
        }
        List<Arma> IProdutoEspecial.armas
        {
            get => DetalheEspecial as List<Arma>;
            set
            {
                if (value != null)
                {
                    DetalheEspecial = value;
                    ProdutoEspecial = new XElement(TiposProduto.Armamento.ToString(),
                        value.ToXElement<List<XElement>>()).ToString(SaveOptions.DisableFormatting);
                }
            }
        }
        Combustivel IProdutoEspecial.comb
        {
            get => DetalheEspecial as Combustivel;
            set
            {
                if (value != null)
                {
                    DetalheEspecial = value;
                    ProdutoEspecial = new XElement(TiposProduto.Combustivel.ToString(),
                        value.ToXElement<Combustivel>()).ToString(SaveOptions.DisableFormatting);
                }
            }
        }
        List<Medicamento> IProdutoEspecial.medicamentos
        {
            get => DetalheEspecial as List<Medicamento>;
            set
            {
                if (value != null)
                {
                    DetalheEspecial = value;
                    ProdutoEspecial = new XElement(TiposProduto.Medicamento.ToString(),
                        value.ToXElement<List<Medicamento>>()).ToString(SaveOptions.DisableFormatting);
                }
            }
        }
        string IProdutoEspecial.NRECOPI
        {
            get => DetalheEspecial as string;
            set
            {
                if (value != null)
                {
                    DetalheEspecial = value;
                    ProdutoEspecial = new XElement(TiposProduto.Papel.ToString(),
                        value).ToString(SaveOptions.DisableFormatting);
                }
            }
        }
        VeiculoNovo IProdutoEspecial.veicProd
        {
            get => DetalheEspecial as VeiculoNovo;
            set
            {
                if (value != null)
                {
                    DetalheEspecial = value;
                    ProdutoEspecial = new XElement(TiposProduto.Veiculo.ToString(),
                        value.ToXElement<VeiculoNovo>()).ToString(SaveOptions.DisableFormatting);
                }
            }
        }

        public string ImpostosSimples { get; set; }
        public string ICMS { get; set; }
        List<ImpSimplesArmazenado> impostosSimples;
        List<ICMSArmazenado> icms;

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
                CEST = string.IsNullOrEmpty(CEST) ? null : CEST
            };
        }

        public void ResetEspecial()
        {
            DetalheEspecial = null;
            ProdutoEspecial = null;
        }

        public IEnumerable<ImpSimplesArmazenado> GetImpSimplesArmazenados()
        {
            if (impostosSimples == null)
            {
                if (!string.IsNullOrEmpty(ImpostosSimples))
                {
                    var xml = XElement.Parse(ImpostosSimples);
                    impostosSimples = xml.FromXElement<List<ImpSimplesArmazenado>>();
                }
                else
                {
                    impostosSimples = new List<ImpSimplesArmazenado>();
                }
            }
            return impostosSimples;
        }

        public IEnumerable<ICMSArmazenado> GetICMSArmazenados()
        {
            if (icms == null)
            {
                if (!string.IsNullOrEmpty(ICMS))
                {
                    var xml = XElement.Parse(ICMS);
                    icms = xml.FromXElement<List<ICMSArmazenado>>();
                }
                else
                {
                    icms = new List<ICMSArmazenado>();
                }
            }
            return icms;
        }

        public void AdicionarImpostoSimples(ImpSimplesArmazenado imp)
        {
            impostosSimples.Add(imp);
            ImpostosSimples = impostosSimples.ToXElement<List<ImpSimplesArmazenado>>()
                .ToString(SaveOptions.DisableFormatting);
        }

        public void AdicionarICMS(ICMSArmazenado imp)
        {
            icms.Add(imp);
            ICMS = icms.ToXElement<List<ICMSArmazenado>>()
                .ToString(SaveOptions.DisableFormatting);
        }

        public void RemoverImpostoSimples(ImpSimplesArmazenado imp)
        {
            impostosSimples.Remove(imp);
            if (impostosSimples.Count > 0)
            {
                ImpostosSimples = impostosSimples.ToXElement<List<ImpSimplesArmazenado>>()
                    .ToString(SaveOptions.DisableFormatting);
            }
            else
            {
                ImpostosSimples = null;
            }
        }

        public void RemoverICMS(ICMSArmazenado imp)
        {
            icms.Remove(imp);
            if (icms.Count > 0)
            {
                ICMS = icms.ToXElement<List<ICMSArmazenado>>()
                    .ToString(SaveOptions.DisableFormatting);
            }
            else
            {
                ICMS = null;
            }
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

        public enum TiposProduto { Simples, Veiculo, Medicamento, Armamento, Combustivel, Papel }
    }
}
