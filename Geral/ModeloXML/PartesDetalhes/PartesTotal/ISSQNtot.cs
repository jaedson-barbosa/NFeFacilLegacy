using NFeFacil.View;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.Generic;
using System.Xml.Serialization;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ModeloXML.PartesDetalhes.PartesTotal
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
                        TryParse(imp.vBC, out double addBC);
                        VBC += addBC;
                        TryParse(imp.vISSQN, out double addISS);
                        vISS += addISS;
                        TryParse(imp.vDeducao, out double addDeducao);
                        VDeducao += addDeducao;
                        TryParse(imp.vDescCond, out double addDescCond);
                        VDescCond += addDescCond;
                        TryParse(imp.vDescIncond, out double addDescIncond);
                        VDescIncond += addDescIncond;
                        TryParse(imp.vISSRet, out double addISSRet);
                        VISSRet += addISSRet;
                    }
                    else
                    {
                        if (imposto is PIS pis)
                        {
                            if (pis.Corpo is PISAliq aliq)
                            {
                                vPIS += Parse(aliq.vPIS);
                            }
                            else if (pis.Corpo is PISQtde qtde)
                            {
                                vPIS += Parse(qtde.vPIS);
                            }
                            else if (pis.Corpo is PISOutr outr)
                            {
                                vPIS += Parse(outr.vPIS);
                            }
                        }
                        else if (imposto is COFINS cofins)
                        {
                            if (cofins.Corpo is COFINSAliq aliq)
                            {
                                vCOFINS += Parse(aliq.vCOFINS);
                            }
                            else if (cofins.Corpo is COFINSQtde qtde)
                            {
                                vCOFINS += Parse(qtde.vCOFINS);
                            }
                            else if (cofins.Corpo is COFINSOutr outr)
                            {
                                vCOFINS += Parse(outr.vCOFINS);
                            }
                        }
                    }
                }
                VServ += prod.ValorTotal;
                TryParse(prod.DespesasAcessorias, out double addOutro);
                VOutro += addOutro;
            }
        }

        [XmlIgnore]
        public double vServ;
        [XmlElement("vServ", Order = 0), DescricaoPropriedade("Valor total do serviços prestados")]
        public string VServ { get => ToStr(vServ); set => vServ = Parse(value); }

        [XmlIgnore]
        public double vBC;
        [XmlElement("vBC", Order = 1), DescricaoPropriedade("Somatório da BC do ISS")]
        public string VBC { get => ToStr(vBC); set => vBC = Parse(value); }

        [XmlIgnore]
        public double vISS;
        [XmlElement("vISS", Order = 2), DescricaoPropriedade("Somatório de ISS")]
        public string VISS { get => ToStr(vISS); set => vISS = Parse(value); }

        double vPIS;
        [XmlElement("vPIS", Order = 3), DescricaoPropriedade("Somatório de PIS")]
        public string VPIS { get => ToStr(vPIS); set => vPIS = Parse(value); }

        double vCOFINS;
        [XmlElement("vCOFINS", Order = 4), DescricaoPropriedade("Somatório de COFINS")]
        public string VCOFINS { get => ToStr(vCOFINS); set => vCOFINS = Parse(value); }

        [XmlElement("dCompet", Order = 5), DescricaoPropriedade("Data da prestação do serviço")]
        public string DCompet { get; set; }

        double vDeducao;
        [XmlElement("vDeducao", Order = 6), DescricaoPropriedade("Valor total da dedução")]
        public string VDeducao { get => ToStr(vDeducao); set => vDeducao = Parse(value); }

        double vOutro;
        [XmlElement("vOutro", Order = 7), DescricaoPropriedade("Valor total de outras retenções")]
        public string VOutro { get => ToStr(vOutro); set => vOutro = Parse(value); }

        double vDescIncond;
        [XmlElement("vDescIncond", Order = 8), DescricaoPropriedade("Valor total do desconto incondicionado")]
        public string VDescIncond { get => ToStr(vDescIncond); set => vDescIncond = Parse(value); }

        double vDescCond;
        [XmlElement("vDescCond", Order = 9), DescricaoPropriedade("Valor total do desconto condicionado")]
        public string VDescCond { get => ToStr(vDescCond); set => vDescCond = Parse(value); }

        double vISSRet;
        [XmlElement("vISSRet", Order = 10), DescricaoPropriedade("Valor total da retenção ISS")]
        public string VISSRet { get => ToStr(vISSRet); set => vISSRet = Parse(value); }

        [XmlElement("cRegTrib", Order = 11), DescricaoPropriedade("Código do Regime Especial de Tributação")]
        public int CRegTrib { get; set; }
    }
}
