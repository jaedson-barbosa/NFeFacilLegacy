using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.Generic;

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
                foreach (var imposto in Produto.impostos.impostos)
                {
                    if (imposto is ISSQN)
                    {
                        var imp = imposto as ISSQN;
                        vBC += imp.vBC.ToDouble();
                        vISS += imp.vISSQN.ToDouble();
                        vDeducao += imp.vDeducao.ToDouble();
                        vDescCond += imp.vDescCond.ToDouble();
                        vDescIncond += imp.vDescIncond.ToDouble();
                        vISSRet += imp.vISSRet.ToDouble();
                    }
                    else
                    {
                        var xmlImposto = imposto.ToXElement(imposto.GetType());
                        if (imposto is PIS)
                        {
                            var alterar = new ConsultarImpostos(xmlImposto);
                            vPIS = alterar.AgregarValor(nameof(vPIS), vPIS);
                        }
                        else if (imposto is COFINS)
                        {
                            var alterar = new ConsultarImpostos(xmlImposto);
                            vCOFINS = alterar.AgregarValor(nameof(vCOFINS), vCOFINS);
                        }
                        else { }
                    }
                }
                vServ += prod.ValorTotal;
                vOutro += prod.DespesasAcessórias.ToDouble();
            }
        }

        /// <summary>
        /// (Opcional)
        /// Informar o valor total do Serviços Pretados, é o somatório dos valores informados em vProd dos itens sujeitos ao ISSQN.
        /// Os valores que sujeitos ao ISSQN deve ter o indTot informado com zero para evitar que o valor seja considerado na validação do somatório do vProd pela SEFAZ.
        /// </summary>
        public double vServ { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório da BC do ISS informado nos itens de Serviços.
        /// </summary>
        public double vBC { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório de ISS informado nos itens de Serviços.
        /// </summary>
        public double vISS { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório de PIS informado nos itens de Serviços.
        /// </summary>
        public double vPIS { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório de COFINS informado nos itens de Serviços.
        /// </summary>
        public double vCOFINS { get; set; }

        /// <summary>
        /// Informar Data da prestação do serviço no formato AAAA-MM-DD.
        /// </summary>
        public string dCompet { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do valor Valor total dedução para redução da Base de Cálculo (vDeducao) informado nos itens.
        /// </summary>
        public double vDeducao { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do valor total Valor total outras retenções (vOutro) informado nos itens. Valor declaratório.
        /// </summary>
        public double vOutro { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do Valor total desconto incondicionado (vDescIncond) informado nos itens.
        /// </summary>
        public double vDescIncond { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do Valor total desconto condicionado (vDescCond) informado nos itens.
        /// </summary>
        public double vDescCond { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o somatório do Valor total retenção ISS (vISSRet) informado nos itens.
        /// </summary>
        public double vISSRet { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informar o Código do Regime Especial de Tributação.
        /// </summary>
        public string cRegTrib { get; set; }
    }
}
