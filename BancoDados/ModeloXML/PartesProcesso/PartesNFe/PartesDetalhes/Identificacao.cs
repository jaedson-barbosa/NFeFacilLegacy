using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public class Identificacao
    {
        public Identificacao() { }
        public Identificacao(Identificacao other)
        {
            CódigoUF = other.CódigoUF;
            ChaveNF = other.ChaveNF;
            NaturezaDaOperacao = other.NaturezaDaOperacao;
            FormaPagamento = other.FormaPagamento;
            Modelo = other.Modelo;
            Serie = other.Serie;
            Numero = other.Numero;
            DataHoraEmissão = other.DataHoraEmissão;
            DataHoraSaídaEntrada = other.DataHoraSaídaEntrada;
            TipoOperacao = other.TipoOperacao;
            IdentificadorDestino = other.IdentificadorDestino;
            CodigoMunicipio = other.CodigoMunicipio;
            TipoImpressão = other.TipoImpressão;
            DígitoVerificador = other.DígitoVerificador;
            TipoAmbiente = other.TipoAmbiente;
            FinalidadeEmissao = other.FinalidadeEmissao;
            OperacaoConsumidorFinal = other.OperacaoConsumidorFinal;
            IndicadorPresenca = other.IndicadorPresenca;
        }

        [XmlElement(ElementName = "cUF", Order = 0)]
        public ushort CódigoUF { get; set; }

        [XmlElement(ElementName = "cNF", Order = 1)]
        public int ChaveNF { get; set; }

        [XmlElement(ElementName = "natOp", Order = 2)]
        public string NaturezaDaOperacao { get; set; }

        [XmlElement(ElementName = "indPag", Order = 3)]
        public ushort FormaPagamento { get; set; } = 0;

        [XmlElement(ElementName = "mod", Order = 4)]
        public ushort Modelo { get; set; } = 55;

        [XmlElement(ElementName = "serie", Order = 5)]
        public ushort Serie { get; set; } = 1;

        [XmlElement(ElementName = "nNF", Order = 6)]
        public int Numero { get; set; }

        [XmlElement(ElementName = "dhEmi", Order = 7)]
        public string DataHoraEmissão { get; set; }

        [XmlElement(ElementName = "dhSaiEnt", Order = 8)]
        public string DataHoraSaídaEntrada { get; set; }

        [XmlElement(ElementName = "tpNF", Order = 9)]
        public ushort TipoOperacao { get; set; } = 1;

        [XmlElement(ElementName = "idDest", Order = 10)]
        public ushort IdentificadorDestino { get; set; } = 1;

        [XmlElement(ElementName = "cMunFG", Order = 11)]
        public int CodigoMunicipio { get; set; }

        [XmlElement(ElementName = "tpImp", Order = 12)]
        public ushort TipoImpressão { get; set; } = 1;

        [XmlElement(ElementName = "tpEmis", Order = 13)]
        public ushort TipoEmissão { get; set; } = 1;

        [XmlElement(ElementName = "cDV", Order = 14)]
        public int DígitoVerificador { get; set; }

        [XmlElement(ElementName = "tpAmb", Order = 15)]
        public ushort TipoAmbiente { get; set; } = 1;

        [XmlElement(ElementName = "finNFe", Order = 16)]
        public ushort FinalidadeEmissao { get; set; } = 1;

        [XmlElement(ElementName = "indFinal", Order = 17)]
        public ushort OperacaoConsumidorFinal { get; set; } = 1;

        [XmlElement(ElementName = "indPres", Order = 18)]
        public ushort IndicadorPresenca { get; set; } = 1;

        [XmlElement(ElementName = "procEmi", Order = 19)]
        public ushort ProcessoEmissão { get; set; } = 0;

        [XmlElement(ElementName = "verProc", Order = 20)]
        public string VersaoAplicativo { get; set; }

        [XmlElement("NFref", Order = 21)]
        public List<DocumentoFiscalReferenciado> DocumentosReferenciados { get; } = new List<DocumentoFiscalReferenciado>();
    }
}
