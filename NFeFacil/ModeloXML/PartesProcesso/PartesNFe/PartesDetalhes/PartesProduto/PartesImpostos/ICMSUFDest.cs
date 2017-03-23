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
            get
            {
                var valores = new List<int> { 40, 60, 80, 100 };
                return pICMSInterPart == null ? -1 : valores.IndexOf(int.Parse(pICMSInterPart));
            }
            set
            {
                var valores = new List<int> { 40, 60, 80, 100 };
                pICMSInterPart = valores[value].ToString();
            }
        }

        [XmlIgnore]
        public int AInterestadual
        {
            get
            {
                var valores = new List<int> { 4, 7, 12 };
                return pICMSInter == null ? -1 : valores.IndexOf(int.Parse(pICMSInter));
            }
            set
            {
                var valores = new List<int> { 4, 7, 12 };
                pICMSInter = valores[value].ToString();
            }
        }

        public override bool IsValido
        {
            get
            {
                return NaoNulos(vBCUFDest, pFCPUFDest, pICMSUFDest, pICMSInter, pICMSInterPart, vFCPUFDest, vICMSUFDest, vICMSUFRemet);
            }
        }
    }
}
