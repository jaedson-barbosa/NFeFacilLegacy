using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal
{
    public sealed class ICMSTot
    {
        public ICMSTot() { }
        /// <summary>
        /// Calcula todos os valores automaticamente com base nos produtos da nota
        /// </summary>
        /// <param name="produtos">Lista com os produtos</param>
        public ICMSTot(IEnumerable<DetalhesProdutos> produtos)
        {
            foreach (var Produto in produtos)
            {
                var temISSQN = false;
                var prod = Produto.Produto;
                foreach (var imposto in Produto.Impostos.impostos)
                {
                    if (imposto is ISSQN)
                    {
                        temISSQN = true;
                    }
                    else
                    {
                        var xmlImposto = imposto.ToXElement(imposto.GetType());
                        if (imposto is ICMS)
                        {
                            var alterar = new ConsultarImpostos(xmlImposto);
                            VBC = alterar.AgregarValor(nameof(VBC), VBC);
                            VICMS = alterar.AgregarValor(nameof(VICMS), VICMS);
                            VICMSDeson = alterar.AgregarValor(nameof(VICMSDeson), VICMSDeson);
                            VFCPUFDest = alterar.AgregarValor(nameof(VFCPUFDest), VFCPUFDest);
                            VICMSUFDest = alterar.AgregarValor(nameof(VICMSUFDest), VICMSUFDest);
                            VICMSUFRemet = alterar.AgregarValor(nameof(VICMSUFRemet), VICMSUFRemet);
                            VBCST = alterar.AgregarValor(nameof(VBCST), VBCST);
                            VST = alterar.AgregarValor("vICMSST", VST);
                        }
                        else if (imposto is II)
                        {
                            VII += double.Parse((imposto as II).vII);
                        }
                        else if (imposto is IPI && (imposto as IPI).Corpo is IPITrib)
                        {
                            var vIPI = ((imposto as IPI).Corpo as IPITrib).vIPI;
                            VIPI += !string.IsNullOrEmpty(vIPI) ? double.Parse(vIPI) : 0;
                        }
                        else if (imposto is PIS)
                        {
                            var alterar = new ConsultarImpostos(xmlImposto);
                            VPIS = alterar.AgregarValor(nameof(VPIS), VPIS);
                        }
                        else if (imposto is COFINS)
                        {
                            var alterar = new ConsultarImpostos(xmlImposto);
                            VCOFINS = alterar.AgregarValor(nameof(VCOFINS), VCOFINS);
                        }
                    }
                }
                if (prod.InclusaoTotal == 1 && !temISSQN)
                {
                    VProd += prod.ValorTotal;
                    VFrete += prod.Frete.ToDouble();
                    VSeg += prod.Seguro.ToDouble();
                    VDesc += prod.Desconto.ToDouble();
                    VOutro += prod.DespesasAcessorias.ToDouble();
                    VTotTrib += Produto.Impostos.vTotTrib.ToDouble();
                }
                else if (temISSQN)
                {
                    vProdISSQN += prod.ValorTotal;
                }
            }
        }

        [XmlElement("vBC", Order = 0), DescricaoPropriedade("Somatório da BC do ICMS")]
        public double VBC { get; set; }

        [XmlElement("vICMS", Order = 1), DescricaoPropriedade("Somatório de ICMS")]
        public double VICMS { get; set; }

        [XmlElement("vICMSDeson", Order = 2), DescricaoPropriedade("Somatório do ICMS desonerado")]
        public double VICMSDeson { get; set; }

        [XmlElement("vFCPUFDest", Order = 3), DescricaoPropriedade("Somatório do ICMS relativo ao FCP da UF destino")]
        public double VFCPUFDest { get;set;}

        [XmlElement("vICMSUFDest", Order = 4), DescricaoPropriedade("Somatório do ICMS interestadual para a UF destino")]
        public double VICMSUFDest { get; set; }

        [XmlElement("vICMSUFRemet", Order = 5), DescricaoPropriedade("Somatório do ICMS interestadual para a UF remetente")]
        public double VICMSUFRemet { get; set; }

        [XmlElement("vBCST", Order = 6), DescricaoPropriedade("Somatório da BC ST")]
        public double VBCST { get; set; }

        [XmlElement("vST", Order = 7), DescricaoPropriedade("Somatório do ICMS ST")]
        public double VST { get; set; }

        [XmlElement("vProd", Order = 8), DescricaoPropriedade("Somatório do valor dos produtos")]
        public double VProd { get; set; }

        [XmlElement("vFrete", Order = 9), DescricaoPropriedade("Somatório de valor do Frete")]
        public double VFrete { get; set; }

        [XmlElement("vSeg", Order = 10), DescricaoPropriedade("Somatório valor do seguro")]
        public double VSeg { get; set; }

        [XmlElement("vDesc", Order = 11), DescricaoPropriedade("Somatório do desconto")]
        public double VDesc { get; set; }

        [XmlElement("vII", Order = 12), DescricaoPropriedade("Somatório de II")]
        public double VII { get; set; }

        [XmlElement("vIPI", Order = 13), DescricaoPropriedade("Somatório de IPI")]
        public double VIPI { get; set; }

        [XmlElement("vPIS", Order = 14), DescricaoPropriedade("Somatório de PIS")]
        public double VPIS { get; set; }

        [XmlElement("vCOFINS", Order = 15), DescricaoPropriedade("Somatório de COFINS")]
        public double VCOFINS { get; set; }

        [XmlElement("vOutro", Order = 16), DescricaoPropriedade("Somatório dos valores adicionais")]
        public double VOutro { get; set; }

        [XmlElement("vNF", Order = 17), DescricaoPropriedade("Valor total da NF")]
        public double VNF
        {
            get
            {
                var valores = new double[] { VProd, VST, VFrete, VSeg, VOutro, VII, VIPI, vProdISSQN};
                return valores.Sum() - (VDesc + VICMSDeson);
            }
            set { }
        }

        [XmlElement("vTotTrib", Order = 18), DescricaoPropriedade("Valor total dos tributos")]
        public double VTotTrib { get; set; }

        double vProdISSQN;
    }
}
