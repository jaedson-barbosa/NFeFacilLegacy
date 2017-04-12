using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao;
using System;
using System.Xml.Serialization;
using Windows.ApplicationModel;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    [PropertyChanged.ImplementPropertyChanged]
    public class Identificacao
    {
        public Identificacao()
        {
            DataHoraEmissão = DateTime.Now.ToString($"yyyy-MM-ddTHH:mm:ss{TimeZoneInfo.Local.BaseUtcOffset.TotalHours}:00");
            DataHoraSaídaEntrada = DataHoraEmissão;
        }
        public Identificacao(Identificacao other)
        {
            CódigoUF = other.CódigoUF;
            ChaveNF = other.ChaveNF;
            NaturezaDaOperação = other.NaturezaDaOperação;
            FormaPagamento = other.FormaPagamento;
            Modelo = other.Modelo;
            Serie = other.Serie;
            Numero = other.Numero;
            DataHoraEmissão = other.DataHoraEmissão;
            DataHoraSaídaEntrada = other.DataHoraSaídaEntrada;
            TipoOperação = other.TipoOperação;
            IdentificadorDestino = other.IdentificadorDestino;
            CodigoMunicípio = other.CodigoMunicípio;
            TipoImpressão = other.TipoImpressão;
            DígitoVerificador = other.DígitoVerificador;
            TipoAmbiente = other.TipoAmbiente;
            FinalidadeEmissão = other.FinalidadeEmissão;
            OperaçãoConsumidorFinal = other.OperaçãoConsumidorFinal;
            IndicadorPresença = other.IndicadorPresença;
        }

        [XmlElement(ElementName = "cUF")]
        public ushort CódigoUF { get; set; }

        [XmlElement(ElementName = "cNF")]
        public string ChaveNF { get; set; }

        [XmlElement(ElementName = "natOp")]
        public string NaturezaDaOperação { get; set; }

        [XmlElement(ElementName = "indPag")]
        public ushort FormaPagamento { get; set; } = 0;

        [XmlElement(ElementName = "mod")]
        public ushort Modelo { get; set; } = 55;

        [XmlElement(ElementName = "serie")]
        public ushort Serie { get; set; } = 1;

        [XmlElement(ElementName = "nNF")]
        public ulong Numero { get; set; }

        [XmlElement(ElementName = "dhEmi")]
        public string DataHoraEmissão { get; set; }

        [XmlElement(ElementName = "dhSaiEnt")]
        public string DataHoraSaídaEntrada { get; set; }

        [XmlElement(ElementName = "tpNF")]
        public ushort TipoOperação { get; set; } = 1;

        [XmlElement(ElementName = "idDest")]
        public ushort IdentificadorDestino { get; set; } = 1;

        [XmlElement(ElementName = "cMunFG")]
        public long CodigoMunicípio { get; set; }

        [XmlElement(ElementName = "tpImp")]
        public ushort TipoImpressão { get; set; } = 1;

        [XmlElement(ElementName = "tpEmis")]
        public ushort TipoEmissão { get; set; } = 1;

        [XmlElement(ElementName = "cDV")]
        public ushort DígitoVerificador { get; set; }

        [XmlElement(ElementName = "tpAmb")]
        public ushort TipoAmbiente { get; set; } = 2;

        [XmlElement(ElementName = "finNFe")]
        public ushort FinalidadeEmissão { get; set; } = 1;

        [XmlElement(ElementName = "indFinal")]
        public ushort OperaçãoConsumidorFinal { get; set; } = 1;

        [XmlElement(ElementName = "indPres")]
        public ushort IndicadorPresença { get; set; } = 1;

        [XmlElement(ElementName = "procEmi")]
        public ushort ProcessoEmissão { get; set; } = 0;

        [XmlElement(ElementName = "verProc")]
        public string VersaoAplicativo { get; set; } = VersãoAplicativo();

        private static string VersãoAplicativo()
        {
            var version = Package.Current.Id.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        [XmlElement("NFref")]
        public DocumentoFiscalReferenciado[] DocumentosReferenciados { get; set; }
    }
}
