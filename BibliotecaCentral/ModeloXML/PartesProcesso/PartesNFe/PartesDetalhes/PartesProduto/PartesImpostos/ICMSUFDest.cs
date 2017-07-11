using System;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class ICMSUFDest : Imposto
    {
        [XmlElement("vBCUFDest", Order = 0)]
        public double VBCUFDest { get; set; }

        [XmlElement("pFCPUFDest", Order = 1)]
        public double PFCPUFDest { get; set; }

        [XmlElement("pICMSUFDest", Order = 2)]
        public double PICMSUFDest { get; set; }

        [XmlElement("pICMSInter", Order = 3)]
        public int PICMSInter { get; set; }

        [XmlElement("pICMSInterPart", Order = 4)]
        public int PICMSInterPart { get; set; }

        [XmlElement("vFCPUFDest", Order = 5)]
        public double VFCPUFDest { get; set; }

        [XmlElement("vICMSUFDest", Order = 6)]
        public double VICMSUFDest { get; set; }

        [XmlElement("vICMSUFRemet", Order = 7)]
        public double VICMSUFRemet { get; set; }

        public override bool IsValido => NaoNulos(VBCUFDest, PFCPUFDest, PICMSUFDest, PICMSInter, PICMSInterPart, VFCPUFDest, VICMSUFDest, VICMSUFRemet);

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
