using System.Collections.Generic;
using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSUFDest : Imposto
    {
        [XmlElement(Order = 0)]
        public string vBCUFDest { get; set; }

        [XmlElement(Order = 1)]
        public string pFCPUFDest { get; set; }

        [XmlElement(Order = 2)]
        public string pICMSUFDest { get; set; }

        [XmlElement(Order = 3)]
        public string pICMSInter { get; set; }

        [XmlElement(Order = 4)]
        public string pICMSInterPart { get; set; }

        [XmlElement(Order = 5)]
        public string vFCPUFDest { get; set; }

        [XmlElement(Order = 6)]
        public string vICMSUFDest { get; set; }

        [XmlElement(Order = 7)]
        public string vICMSUFRemet { get; set; }

        [XmlIgnore]
        public int PPPartilha
        {
            get => pICMSInterPart == null ? -1 : new List<int> { 40, 60, 80, 100 }.IndexOf(int.Parse(pICMSInterPart));
            set => pICMSInterPart = new List<int> { 40, 60, 80, 100 }[value].ToString();
        }

        [XmlIgnore]
        public int AInterestadual
        {
            get => pICMSInter == null ? -1 : new List<int> { 4, 7, 12 }.IndexOf(int.Parse(pICMSInter));
            set => pICMSInter = new List<int> { 4, 7, 12 }[value].ToString();
        }

        public override bool IsValido => NaoNulos(vBCUFDest, pFCPUFDest, pICMSUFDest, pICMSInter, pICMSInterPart, vFCPUFDest, vICMSUFDest, vICMSUFRemet);
    }
}
