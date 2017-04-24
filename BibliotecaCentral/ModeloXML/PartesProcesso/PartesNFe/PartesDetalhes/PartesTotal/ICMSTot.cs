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
                foreach (var imposto in Produto.impostos.impostos)
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
                            vBC = alterar.AgregarValor(nameof(vBC), vBC);
                            vICMS = alterar.AgregarValor(nameof(vICMS), vICMS);
                            vICMSDeson = alterar.AgregarValor(nameof(vICMSDeson), vICMSDeson);
                            vBCST = alterar.AgregarValor(nameof(vBCST), vBCST);
                            vST = alterar.AgregarValor("vICMSST", vST);
                        }
                        else if (imposto is II)
                        {
                            vII += double.Parse((imposto as II).vII);
                        }
                        else if (imposto is IPI && (imposto as IPI).Corpo is IPITrib)
                        {
                            var vIPI = ((imposto as IPI).Corpo as IPITrib).vIPI;
                            vIPI += vIPI != null ? double.Parse(vIPI) : 0;
                        }
                        else if (imposto is PIS)
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
                if (prod.InclusãoTotal == 1 && !temISSQN)
                {
                    vProd += prod.ValorTotal;
                    vFrete += prod.Frete.ToDouble();
                    vSeg += prod.Seguro.ToDouble();
                    vDesc += prod.Desconto.ToDouble();
                    vOutro += prod.DespesasAcessórias.ToDouble();
                    vTotTrib += Produto.impostos.vTotTrib.ToDouble();
                }
                else if (temISSQN)
                {
                    vProdISSQN += prod.ValorTotal;
                }
                else { }
            }
        }

        /// <summary>
        /// Informar o somatório da BC do ICMS (vBC) informado nos itens.
        /// </summary>
        public double vBC { get; set; }

        /// <summary>
        /// Informar o somatório de ICMS (vICMS) informado nos itens.
        /// </summary>
        public double vICMS { get; set; }

        /// <summary>
        /// Informar o somatório do Valor do ICMS desonerado (vICMSDeson) informado nos itens.
        /// </summary>
        public double vICMSDeson { get; set; }

        /// <summary>
        /// Informar o somatório da BC ST (vBCST) informado nos itens.
        /// </summary>
        public double vBCST { get; set; }

        /// <summary>
        /// Informar o somatório do ICMS ST (vICMSST)informado nos itens.
        /// </summary>
        public double vST { get; set; }

        /// <summary>
        /// Informar o somatório de valor dos produtos (vProd) dos itens que tenham indicador de totalização = 1 (indTot).
        /// Os valores dos itens sujeitos ao ISSQN não devem ser acumulados neste campo.
        /// </summary>
        public double vProd { get; set; }

        /// <summary>
        /// Informar o somatório de valor do Frete (vFrete) informado nos itens.
        /// </summary>
        public double vFrete { get; set; }

        /// <summary>
        /// Informar o somatório valor do Seguro (vSeg) informado nos itens.
        /// </summary>
        public double vSeg { get; set; }

        /// <summary>
        /// Informar o somatório do Desconto (vDesc) informado nos itens.
        /// </summary>
        public double vDesc { get; set; }

        /// <summary>
        /// Informar o somatório de II (vII) informado nos itens.
        /// </summary>
        public double vII { get; set; }

        /// <summary>
        /// Informar o somatório de IPI (vIPI) informado nos itens.
        /// </summary>
        public double vIPI { get; set; }

        /// <summary>
        /// Informar o somatório de PIS (vPIS) informado nos itens sujeitos ao ICMS.
        /// </summary>
        public double vPIS { get; set; }

        /// <summary>
        /// Informar o somatório de PIS (vPIS) informado nos itens sujeitos ao ICMS.
        /// </summary>
        public double vCOFINS { get; set; }

        /// <summary>
        /// Informar o somatório de vOutro (vOutro) informado nos itens.
        /// </summary>
        public double vOutro { get; set; }

        /// <summary>
        /// Informar o valor total a NF.
        /// Acrescentar o valor dos Serviços informados no grupo do ISSQN.
        /// </summary>
        public double vNF
        {
            get
            {
                var valores = new List<double> { vBC, vICMS, vICMSDeson, vBCST, vST, vProd, vFrete, vSeg, vII, vIPI, vPIS, vCOFINS, vOutro, vTotTrib, vProdISSQN };
                return valores.Sum() - vDesc;
            }
            set { }
        }

        /// <summary>
        /// (Opcional)
        /// informar o somatório do valor total aproximado dos tributos (vTotTrib) informado nos itens.
        /// Deve considerar valor de itens sujeitos ao ISSQN também.
        /// </summary>
        public double vTotTrib { get; set; }

        [XmlIgnore]
        private double vProdISSQN { get; set; }
    }
}
