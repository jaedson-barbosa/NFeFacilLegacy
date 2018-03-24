using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class ISSQN : ImpostoBase
    {
        [XmlElement(Order = 0), DescricaoPropriedade("Informar o Valor da BC do ISSQN")]
        public string vBC { get; set; }

        [XmlElement(Order = 1), DescricaoPropriedade("Informar a Alíquota do ISSQN")]
        public string vAliq { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Informar o Valor do ISSQN")]
        public string vISSQN { get; set; }

        [XmlElement(Order = 3), DescricaoPropriedade("Informar o código do município de ocorrência do fato gerador do ISSQN")]
        public string cMunFG { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Informar Informar o Item da lista de serviços em que se classifica o serviço no padrão ABRASF")]
        public string cListServ { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Informar o Valor dedução para redução da Base de Cálculo")]
        public string vDeducao { get; set; }

        [XmlElement(Order = 6), DescricaoPropriedade("Informar o Valor outras retenções")]
        public string vOutro { get; set; }

        [XmlElement(Order = 7), DescricaoPropriedade("Informar o Valor desconto incondicionado")]
        public string vDescIncond { get; set; }

        [XmlElement(Order = 8), DescricaoPropriedade("Valor desconto condicionado")]
        public string vDescCond { get; set; }

        [XmlElement(Order = 9), DescricaoPropriedade("Informar o Valor retenção ISS")]
        public string vISSRet { get; set; }

        [XmlElement(Order = 10), DescricaoPropriedade("Informar Indicador da exigibilidade do ISS")]
        public string indISS { get; set; }

        [XmlElement(Order = 11), DescricaoPropriedade("Informar o Código do serviço prestado dentro do município")]
        public string cServico { get; set; }

        [XmlElement(Order = 12), DescricaoPropriedade("Informar o código do município de ocorrência do fato gerador do ISSQN")]
        public string cMun { get; set; }

        [XmlElement(Order = 13), DescricaoPropriedade("Informar o Código do País onde o serviço foi prestado")]
        public string cPais { get; set; }

        [XmlElement(Order = 14), DescricaoPropriedade("Informar o Número do processo judicial ou administrativo de suspensão da exigibilidade")]
        public string nProcesso { get; set; }

        [XmlElement(Order = 15), DescricaoPropriedade("Informar Indicador de incentivo Fiscal")]
        public string indIncentivo { get; set; }
    }
}
