using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao;
using System;
using System.Xml.Serialization;
using Windows.ApplicationModel;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Identificacao
    {
        public Identificacao()
        {
            DataHoraSaídaEntrada = DataHoraEmissão = DateTime.Now.ToStringPersonalizado();
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

        [XmlElement(ElementName = "cUF", Order = 0)]
        public ushort CódigoUF { get; set; }

        [XmlElement(ElementName = "cNF", Order = 1)]
        public string ChaveNF { get; set; }

        [XmlElement(ElementName = "natOp", Order = 2)]
        public string NaturezaDaOperação { get; set; }

        [XmlElement(ElementName = "indPag", Order = 3)]
        public ushort FormaPagamento { get; set; } = 0;

        [XmlElement(ElementName = "mod", Order = 4)]
        public ushort Modelo { get; set; } = 55;

        [XmlElement(ElementName = "serie", Order = 5)]
        public ushort Serie { get; set; } = 1;

        [XmlElement(ElementName = "nNF", Order = 6)]
        public long Numero { get; set; }

        [XmlElement(ElementName = "dhEmi", Order = 7)]
        public string DataHoraEmissão { get; set; }

        [XmlElement(ElementName = "dhSaiEnt", Order = 8)]
        public string DataHoraSaídaEntrada { get; set; }

        [XmlElement(ElementName = "tpNF", Order = 9)]
        public ushort TipoOperação { get; set; } = 1;

        [XmlElement(ElementName = "idDest", Order = 10)]
        public ushort IdentificadorDestino { get; set; } = 1;

        [XmlElement(ElementName = "cMunFG", Order = 11)]
        public int CodigoMunicípio { get; set; }

        [XmlElement(ElementName = "tpImp", Order = 12)]
        public ushort TipoImpressão { get; set; } = 1;

        [XmlElement(ElementName = "tpEmis", Order = 13)]
        public ushort TipoEmissão { get; set; } = 1;

        [XmlElement(ElementName = "cDV", Order = 14)]
        public int DígitoVerificador { get; set; }

        [XmlElement(ElementName = "tpAmb", Order = 15)]
        public ushort TipoAmbiente { get; set; } = 2;

        [XmlElement(ElementName = "finNFe", Order = 16)]
        public ushort FinalidadeEmissão { get; set; } = 1;

        [XmlElement(ElementName = "indFinal", Order = 17)]
        public ushort OperaçãoConsumidorFinal { get; set; } = 1;

        [XmlElement(ElementName = "indPres", Order = 18)]
        public ushort IndicadorPresença { get; set; } = 1;

        [XmlElement(ElementName = "procEmi", Order = 19)]
        public ushort ProcessoEmissão { get; set; } = 0;

        [XmlElement(ElementName = "verProc", Order = 20)]
        public string VersaoAplicativo { get; set; } = VersãoAplicativo();

        private static string VersãoAplicativo()
        {
            var version = Package.Current.Id.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        [XmlElement("NFref", Order = 21)]
        public DocumentoFiscalReferenciado[] DocumentosReferenciados { get; set; }
    }
}
