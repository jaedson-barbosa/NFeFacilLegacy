using BaseGeral.View;
using System.Xml.Serialization;
using static BaseGeral.ExtensoesPrincipal;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// O preenchimento dos campos do grupo de ICMS são variáveis e dependem do CST ou do CSOSN do item de Produto.
    /// Por causa do tamanho desta classe será usada uma solução do tipo "gambiarra".
    /// </summary>
    public abstract class ComumICMS
    {
        #region Comum

        [XmlElement("orig", Order = 0), DescricaoPropriedade("Origem da mercadoria")]
        public int Origem { get; set; }

        [XmlElement("vBCSTRet", Order = 3), DescricaoPropriedade("Valor da BC do ICMS Retido Anteriormente")]
        public string vBCSTRet { get; set; }

        [XmlElement("vICMSSTRet", Order = 5), DescricaoPropriedade("Valor do ICMS Retido Anteriormente")]
        public string vICMSSTRet { get; set; }

        [XmlElement("modBC", Order = 8), DescricaoPropriedade("Modalidade de determinação da BC do ICMS")]
        public string modBC { get; set; }

        [XmlElement("vBC", Order = 9), DescricaoPropriedade("Valor da BC do ICMS")]
        public string vBC { get; set; }

        [XmlElement("pRedBC", Order = 10), DescricaoPropriedade("Percentual da Redução de BC")]
        public string pRedBC { get; set; }

        [XmlElement("pICMS", Order = 11), DescricaoPropriedade("Alíquota do imposto")]
        public string pICMS { get; set; }

        [XmlElement("vICMS", Order = 15), DescricaoPropriedade("Valor do ICMS")]
        public string vICMS { get; set; }

        [XmlElement("modBCST", Order = 16), DescricaoPropriedade("Modalidade de determinação da BC do ICMS ST")]
        public string modBCST { get; set; }

        [XmlElement("pMVAST", Order = 17), DescricaoPropriedade("Percentual da margem de valor Adicionado do ICMS ST")]
        public string pMVAST { get; set; }

        [XmlElement("pRedBCST", Order = 18), DescricaoPropriedade("Percentual da Redução de BC do ICMS ST")]
        public string pRedBCST { get; set; }

        [XmlElement("vBCST", Order = 19), DescricaoPropriedade("Valor da BC do ICMS ST")]
        public string vBCST { get; set; }

        [XmlElement("pICMSST", Order = 20), DescricaoPropriedade("Alíquota do imposto do ICMS ST")]
        public string pICMSST { get; set; }

        [XmlElement("vICMSST", Order = 21), DescricaoPropriedade("Valor do ICMS ST")]
        public string vICMSST { get; set; }

        #endregion

        #region Registro normal

        [XmlElement(Order = 1), DescricaoPropriedade("Tributação do ICMS")]
        public string CST { get; set; }

        [XmlElement("pST", Order = 4), DescricaoPropriedade("Alíquota suportada pelo Consumidor Final")]
        public string pST { get; set; }

        [XmlElement("vICMSOp", Order = 12), DescricaoPropriedade("Valor do ICMS da Operação")]
        public string vICMSOp { get; set; }

        [XmlElement("pDif", Order = 13), DescricaoPropriedade("Percentual do diferimento")]
        public string pDif { get; set; }

        [XmlElement("vICMSDif", Order = 14), DescricaoPropriedade("Valor do ICMS Diferido")]
        public string vICMSDif { get; set; }

        [XmlElement("vICMSDeson", Order = 24), DescricaoPropriedade("Valor do ICMS da desoneração")]
        public string vICMSDeson { get; set; }

        [XmlElement("motDesICMS", Order = 25), DescricaoPropriedade("Motivo da desoneração do ICMS")]
        public string motDesICMS { get; set; }

        [XmlElement("pBCOp", Order = 26), DescricaoPropriedade("Percentual da BC operação própria")]
        public string pBCOp { get; set; }

        [XmlElement("UFST", Order = 27), DescricaoPropriedade("UF para qual é devido o ICMS ST")]
        public string UFST { get; set; }

        #endregion

        #region Simples nacional

        [XmlElement(Order = 2), DescricaoPropriedade("Código de Situação da Operação – Simples Nacional")]
        public string CSOSN { get; set; }

        [XmlElement("vBCSTDest", Order = 6), DescricaoPropriedade("Valor da BC do ICMS ST da UF destino")]
        public string vBCSTDest { get; set; }

        [XmlElement("vICMSSTDest", Order = 7), DescricaoPropriedade("Valor do ICMS ST da UF destino")]
        public string vICMSSTDest { get; set; }

        [XmlElement("pCredSN", Order = 22), DescricaoPropriedade("Alíquota aplicável de cálculo do crédito (Simples Nacional)")]
        public string pCredSN { get; set; }

        [XmlElement("vCredICMSSN", Order = 23), DescricaoPropriedade("Valor crédito do ICMS que pode ser aproveitado nos termos do art. 23 da LC 123 (SIMPLES NACIONAL)")]
        public string vCredICMSSN { get; set; }

        #endregion

        protected ComumICMS() { }

        protected ComumICMS(int origem, string cstOuCsosn, bool isSimples)
        {
            Origem = origem;
            if (isSimples) CSOSN = cstOuCsosn;
            else CST = cstOuCsosn;
        }

        protected double CalcularBC(DetalhesProdutos detalhes) => detalhes.Produto.ValorTotal
            + detalhes.Produto.Frete
            + detalhes.Produto.Seguro
            + detalhes.Produto.DespesasAcessorias
            - detalhes.Produto.Desconto;

        protected double ObterIPI(DetalhesProdutos detalhes)
        {
            var impCriados = detalhes.Impostos.impostos;
            for (int i = 0; i < impCriados.Count; i++)
                if (impCriados[i] is IPI ipi && ipi.Corpo is IPITrib trib && !string.IsNullOrEmpty(trib.ValorIPI))
                    return Parse(trib.ValorIPI);
            return 0;
        }
    }
}
