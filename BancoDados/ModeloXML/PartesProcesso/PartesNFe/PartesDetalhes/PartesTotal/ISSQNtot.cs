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
                VServ += prod.ValorTotal;
                VOutro += prod.DespesasAcessórias.ToDouble();
            }
        }

        /// <summary>
        /// (Opcional)
        /// Informar o valor total do Serviços Pretados, é o somatório dos valores informados em vProd dos itens sujeitos ao ISSQN.
        /// Os valores que sujeitos ao ISSQN deve ter o indTot informado com zero para evitar que o valor seja considerado na validação do somatório do vProd pela SEFAZ.
        /// </summary>
        [XmlElement("vServ", Order = 0)]
        public double VServ { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório da BC do ISS informado nos itens de Serviços.
        /// </summary>
        [XmlElement("vBC", Order = 1)]
        public double VBC { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório de ISS informado nos itens de Serviços.
        /// </summary>
        [XmlElement("vISS", Order = 2)]
        public double VISS { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório de PIS informado nos itens de Serviços.
        /// </summary>
        [XmlElement("vPIS", Order = 3)]
        public double VPIS { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório de COFINS informado nos itens de Serviços.
        /// </summary>
        [XmlElement("vCOFINS", Order = 4)]
        public double VCOFINS { get; set; }

        /// <summary>
        /// Informar Data da prestação do serviço no formato AAAA-MM-DD.
        /// </summary>
        [XmlElement("dCompet", Order = 5)]
        public string DCompet { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do valor Valor total dedução para redução da Base de Cálculo (vDeducao) informado nos itens.
        /// </summary>
        [XmlElement("vDeducao", Order = 6)]
        public double VDeducao { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do valor total Valor total outras retenções (vOutro) informado nos itens. Valor declaratório.
        /// </summary>
        [XmlElement("vOutro", Order = 7)]
        public double VOutro { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do Valor total desconto incondicionado (vDescIncond) informado nos itens.
        /// </summary>
        [XmlElement("vDescIncond", Order = 8)]
        public double VDescIncond { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do Valor total desconto condicionado (vDescCond) informado nos itens.
        /// </summary>
        [XmlElement("vDescCond", Order = 9)]
        public double VDescCond { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do Valor total retenção ISS (vISSRet) informado nos itens.
        /// </summary>
        [XmlElement("vISSRet", Order = 10)]
        public double VISSRet { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Código do Regime Especial de Tributação.
        /// </summary>
        [XmlElement("cRegTrib", Order = 11)]
        public string CRegTrib { get; set; }
    }
}
