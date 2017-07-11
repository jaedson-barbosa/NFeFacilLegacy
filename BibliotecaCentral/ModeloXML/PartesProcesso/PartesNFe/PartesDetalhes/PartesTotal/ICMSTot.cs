using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal
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
                if (prod.InclusãoTotal == 1 && !temISSQN)
                {
                    VProd += prod.ValorTotal;
                    VFrete += prod.Frete.ToDouble();
                    VSeg += prod.Seguro.ToDouble();
                    VDesc += prod.Desconto.ToDouble();
                    VOutro += prod.DespesasAcessórias.ToDouble();
                    VTotTrib += Produto.Impostos.vTotTrib.ToDouble();
                }
                else if (temISSQN)
                {
                    vProdISSQN += prod.ValorTotal;
                }
            }
        }

        /// <summary>
        /// Informar o somatório da BC do ICMS (vBC) informado nos itens.
        /// </summary>
        [XmlElement("vBC", Order = 0)]
        public double VBC { get; set; }

        /// <summary>
        /// Informar o somatório de ICMS (vICMS) informado nos itens.
        /// </summary>
        [XmlElement("vICMS", Order = 1)]
        public double VICMS { get; set; }

        /// <summary>
        /// Informar o somatório do Valor do ICMS desonerado (vICMSDeson) informado nos itens.
        /// </summary>
        [XmlElement("vICMSDeson", Order = 2)]
        public double VICMSDeson { get; set; }

        [XmlElement("vFCPUFDest", Order = 3)]
        public string VFCPUFDest { get;set;}

        [XmlElement("vICMSUFDest", Order = 4)]
        public string VICMSUFDest { get; set; }

        [XmlElement("vICMSUFRemet", Order = 5)]
        public string VICMSUFRemet { get; set; }

        /// <summary>
        /// Informar o somatório da BC ST (vBCST) informado nos itens.
        /// </summary>
        [XmlElement("vBCST", Order = 6)]
        public double VBCST { get; set; }

        /// <summary>
        /// Informar o somatório do ICMS ST (vICMSST)informado nos itens.
        /// </summary>
        [XmlElement("vST", Order = 7)]
        public double VST { get; set; }

        /// <summary>
        /// Informar o somatório de valor dos produtos (vProd) dos itens que tenham indicador de totalização = 1 (indTot).
        /// Os valores dos itens sujeitos ao ISSQN não devem ser acumulados neste campo.
        /// </summary>
        [XmlElement("vProd", Order = 8)]
        public double VProd { get; set; }

        /// <summary>
        /// Informar o somatório de valor do Frete (vFrete) informado nos itens.
        /// </summary>
        [XmlElement("vFrete", Order = 9)]
        public double VFrete { get; set; }

        /// <summary>
        /// Informar o somatório valor do Seguro (vSeg) informado nos itens.
        /// </summary>
        [XmlElement("vSeg", Order = 10)]
        public double VSeg { get; set; }

        /// <summary>
        /// Informar o somatório do Desconto (vDesc) informado nos itens.
        /// </summary>
        [XmlElement("vDesc", Order = 11)]
        public double VDesc { get; set; }

        /// <summary>
        /// Informar o somatório de II (vII) informado nos itens.
        /// </summary>
        [XmlElement("vII", Order = 12)]
        public double VII { get; set; }

        /// <summary>
        /// Informar o somatório de IPI (vIPI) informado nos itens.
        /// </summary>
        [XmlElement("vIPI", Order = 13)]
        public double VIPI { get; set; }

        /// <summary>
        /// Informar o somatório de PIS (vPIS) informado nos itens sujeitos ao ICMS.
        /// </summary>
        [XmlElement("vPIS", Order = 14)]
        public double VPIS { get; set; }

        /// <summary>
        /// Informar o somatório de PIS (vPIS) informado nos itens sujeitos ao ICMS.
        /// </summary>
        [XmlElement("vCOFINS", Order = 15)]
        public double VCOFINS { get; set; }

        /// <summary>
        /// Informar o somatório de vOutro (vOutro) informado nos itens.
        /// </summary>
        [XmlElement("vOutro", Order = 16)]
        public double VOutro { get; set; }

        /// <summary>
        /// Informar o valor total a NF.
        /// Acrescentar o valor dos Serviços informados no grupo do ISSQN.
        /// </summary>
        [XmlElement("vNF", Order = 17)]
        public double VNF
        {
            get
            {
                var valores = new double[] { VProd, VST, VFrete, VSeg, VOutro, VII, VIPI, vProdISSQN};
                return valores.Sum() - (VDesc + VICMSDeson);
            }
            set { }
        }

        /// <summary>
        /// (Opcional)
        /// informar o somatório do valor total aproximado dos tributos (vTotTrib) informado nos itens.
        /// Deve considerar valor de itens sujeitos ao ISSQN também.
        /// </summary>
        [XmlElement("vTotTrib", Order = 18)]
        public double VTotTrib { get; set; }

        double vProdISSQN;
    }
}
