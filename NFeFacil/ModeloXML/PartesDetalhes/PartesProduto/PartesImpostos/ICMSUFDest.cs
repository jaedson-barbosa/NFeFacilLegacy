using NFeFacil.View;
using System;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class ICMSUFDest : ImpostoBase
    {
        [XmlElement("vBCUFDest", Order = 0), DescricaoPropriedade("Valor da BC do ICMS na UF destino")]
        public double VBCUFDest { get; set; }

        [XmlElement("pFCPUFDest", Order = 1), DescricaoPropriedade("Percentual adicional inserido na alíquota interna da UF destino relativo ao FCP naquela UF")]
        public double PFCPUFDest { get; set; }

        [XmlElement("pICMSUFDest", Order = 2), DescricaoPropriedade("Alíquota adotada nas operações internas na UF destino")]
        public double PICMSUFDest { get; set; }

        [XmlElement("pICMSInter", Order = 3), DescricaoPropriedade("Alíquota interestadual das UF envolvidas")]
        public int PICMSInter { get; set; }

        [XmlElement("pICMSInterPart", Order = 4), DescricaoPropriedade("Percentual de ICMS interestadual para a UF destino")]
        public int PICMSInterPart { get; set; }

        [XmlElement("vFCPUFDest", Order = 5), DescricaoPropriedade("Valor do ICMS relativo ao FCP da UF destino")]
        public double VFCPUFDest { get; set; }

        [XmlElement("vICMSUFDest", Order = 6), DescricaoPropriedade("Valor do ICMS interestadual para a UF destino")]
        public double VICMSUFDest { get; set; }

        [XmlElement("vICMSUFRemet", Order = 7), DescricaoPropriedade("Valor do ICMS interestadual para a UF remetente")]
        public double VICMSUFRemet { get; set; }

        public ICMSUFDest()
        {
            switch (DateTime.Now.Year)
            {
                case 2016:
                    PICMSInterPart = 40;
                    break;
                case 2017:
                    PICMSInterPart = 60;
                    break;
                case 2018:
                    PICMSInterPart = 80;
                    break;
                default:
                    PICMSInterPart = 100;
                    break;
            }
        }
    }
}
