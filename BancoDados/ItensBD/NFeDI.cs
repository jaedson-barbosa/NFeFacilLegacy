using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ItensBD
{
    public sealed class NFeDI
    {
        public string Id { get; set; }
        public DateTime UltimaData { get; set; }

        [Required]
        public int NumeroNota { get; set; }
        [Required]
        public ushort SerieNota { get; set; }
        [Required]
        public string NomeEmitente { get; set; }
        [Required]
        public string CNPJEmitente { get; set; }
        [Required]
        public string NomeCliente { get; set; }
        [Required]
        public string DataEmissao { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string XML { get; set; }

        public bool Impressa { get; set; }
        public bool Exportada { get; set; }

        public static NFeDI Converter(XElement xml)
        {
            if (xml.Name.LocalName == nameof(NFe))
            {
                return new NFeDI(FromXElement<NFe>(xml), xml.ToString());
            }
            else
            {
                return new NFeDI(FromXElement<Processo>(xml), xml.ToString());
            }
        }

        public NFeDI() { }

        public NFeDI(NFe nota, string xml)
        {
            Id = nota.Informacoes.Id;
            NomeCliente = nota.Informacoes.destinatário.Nome;
            NomeEmitente = nota.Informacoes.emitente.Nome;
            CNPJEmitente = nota.Informacoes.emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.Informacoes.identificacao.Numero;
            SerieNota = nota.Informacoes.identificacao.Serie;
            Status = nota.Signature != null && nota.Signature != null ? (int)StatusNFe.Assinada : (int)StatusNFe.Salva;
            XML = xml;
        }

        public NFeDI(Processo nota, string xml)
        {
            Id = nota.NFe.Informacoes.Id;
            NomeCliente = nota.NFe.Informacoes.destinatário.Nome;
            NomeEmitente = nota.NFe.Informacoes.emitente.Nome;
            CNPJEmitente = nota.NFe.Informacoes.emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.NFe.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.NFe.Informacoes.identificacao.Numero;
            SerieNota = nota.NFe.Informacoes.identificacao.Serie;
            Status = nota.ProtNFe != null ? (int)StatusNFe.Emitida : nota.NFe.Signature != null ? (int)StatusNFe.Assinada : (int)StatusNFe.Salva;
            XML = xml;
        }

        static T FromXElement<T>(XNode xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = xElement.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }
    }
}
