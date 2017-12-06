using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal
{
    public sealed class ISSQNtot
    {
        public ISSQNtot() { }
        /// <summary>
        /// Calcula todos os valores automaticamente com base nos produtos da nota
        /// </summary>
        /// <param name="produtos">Lista com os produtos</param>
        public ISSQNtot(IEnumerable<DetalhesProdutos> produtos)
        {
            foreach (var Produto in produtos)
            {
                var prod = Produto.Produto;
                foreach (var imposto in Produto.Impostos.impostos)
                {
                    if (imposto is ISSQN)
                    {
                        var imp = imposto as ISSQN;
                        VBC += imp.vBC.ToDouble();
                        VISS += imp.vISSQN.ToDouble();
                        VDeducao += imp.vDeducao.ToDouble();
                        VDescCond += imp.vDescCond.ToDouble();
                        VDescIncond += imp.vDescIncond.ToDouble();
                        VISSRet += imp.vISSRet.ToDouble();
                    }
                    else
                    {
                        var xmlImposto = imposto.ToXElement(imposto.GetType());
                        if (imposto is PIS)
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
                VServ += prod.valorTotal;
                VOutro += prod.DespesasAcessorias.ToDouble();
            }
        }

        [XmlElement("vServ", Order = 0), DescricaoPropriedade("Valor total do serviços prestados")]
        public double VServ { get; set; }

        [XmlElement("vBC", Order = 1), DescricaoPropriedade("Somatório da BC do ISS")]
        public double VBC { get; set; }

        [XmlElement("vISS", Order = 2), DescricaoPropriedade("Somatório de ISS")]
        public double VISS { get; set; }

        [XmlElement("vPIS", Order = 3), DescricaoPropriedade("Somatório de PIS")]
        public double VPIS { get; set; }

        [XmlElement("vCOFINS", Order = 4), DescricaoPropriedade("Somatório de COFINS")]
        public double VCOFINS { get; set; }

        [XmlElement("dCompet", Order = 5), DescricaoPropriedade("Data da prestação do serviço")]
        public string DCompet { get; set; }

        [XmlElement("vDeducao", Order = 6), DescricaoPropriedade("Valor total da dedução")]
        public double VDeducao { get; set; }

        [XmlElement("vOutro", Order = 7), DescricaoPropriedade("Valor total de outras retenções")]
        public double VOutro { get; set; }

        [XmlElement("vDescIncond", Order = 8), DescricaoPropriedade("Valor total do desconto incondicionado")]
        public double VDescIncond { get; set; }

        [XmlElement("vDescCond", Order = 9), DescricaoPropriedade("Valor total do desconto condicionado")]
        public double VDescCond { get; set; }

        [XmlElement("vISSRet", Order = 10), DescricaoPropriedade("Valor total da retenção ISS")]
        public double VISSRet { get; set; }

        [XmlElement("cRegTrib", Order = 11), DescricaoPropriedade("Código do Regime Especial de Tributação")]
        public string CRegTrib { get; set; }
    }
}
