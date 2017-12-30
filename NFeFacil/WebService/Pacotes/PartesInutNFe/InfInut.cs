using System.Xml.Serialization;

namespace NFeFacil.WebService.Pacotes.PartesInutNFe
{
    public struct InfInut
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement("tpAmb", Order = 0)]
        public int TipoAmbiente { get; set; }

        [XmlElement("xServ", Order = 1)]
        public string DescricaoServico { get; set; }

        [XmlElement("cUF", Order = 2)]
        public int CodigoUF { get; set; }

        [XmlElement("ano", Order = 3)]
        public int Ano { get; set; }

        [XmlElement(Order = 4)]
        public string CNPJ { get; set; }

        [XmlElement("mod", Order = 5)]
        public int ModeloDocumento { get; set; }

        [XmlElement("serie", Order = 6)]
        public int SerieNFe { get; set; }

        [XmlElement("nNFIni", Order = 7)]
        public int InicioNumeracao { get; set; }

        [XmlElement("nNFFin", Order = 8)]
        public int FinalNumeracao { get; set; }

        [XmlElement("xJust", Order = 9)]
        public string Justificativa { get; set; }

        public InfInut(bool homologacao, int serieNFe, int inicioNum, int fimNum, string justificativa)
        {
            TipoAmbiente = homologacao ? 2 : 1;
            DescricaoServico = "INUTILIZAR";
            var emit = DefinicoesTemporarias.EmitenteAtivo;
            CodigoUF = IBGE.Estados.Buscar(emit.SiglaUF).Codigo;
            Ano = DefinicoesTemporarias.DateTimeNow.Year - 2000;
            CNPJ = emit.CNPJ;
            ModeloDocumento = 55;
            SerieNFe = serieNFe;
            InicioNumeracao = inicioNum;
            FinalNumeracao = fimNum;
            Justificativa = justificativa;
            Id = $"ID{CodigoUF}{Ano}{CNPJ}{ModeloDocumento}{SerieNFe.ToString("000")}{inicioNum.ToString("000000000")}{fimNum.ToString("000000000")}";
        }
    }
}
