using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSUFDest : Imposto
    {
        public string vBCUFDest { get; set; }
        public string pFCPUFDest { get; set; }
        public string pICMSUFDest { get; set; }
        public string pICMSInter { get; set; }
        public string pICMSInterPart { get; set; }
        public string vFCPUFDest { get; set; }
        public string vICMSUFDest { get; set; }
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
