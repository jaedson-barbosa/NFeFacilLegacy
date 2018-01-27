using NFeFacil.ModeloXML;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ItensBD
{
    public sealed class NFeDI : IUltimaData, IStatusAtual
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

        public int StatusAdd => (int)StatusNota.Salva;
        public bool IsNFCe { get; set; }

        public static NFeDI Converter(XElement xml)
        {
            if (xml.Name.LocalName == nameof(NFe))
            {
                return new NFeDI(FromXElement<NFe>(xml), xml.ToString());
            }
            else
            {
                return new NFeDI(FromXElement<ProcessoNFe>(xml), xml.ToString());
            }
        }

        public NFeDI() { }

        public NFeDI(NFe nota, string xml)
        {
            Id = nota.Informacoes.Id;
            NomeCliente = nota.Informacoes.destinatário.Nome;
            NomeEmitente = nota.Informacoes.Emitente.Nome;
            CNPJEmitente = nota.Informacoes.Emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.Informacoes.identificacao.Numero;
            SerieNota = nota.Informacoes.identificacao.Serie;
            Status = nota.Signature != null && nota.Signature != null ? (int)StatusNota.Assinada : (int)StatusNota.Salva;
            IsNFCe = nota.Informacoes.identificacao.Modelo == 65;
            XML = xml;
        }

        public NFeDI(NFCe nota, string xml)
        {
            Id = nota.Informacoes.Id;
            NomeCliente = nota.Informacoes.destinatário.Nome;
            NomeEmitente = nota.Informacoes.Emitente.Nome;
            CNPJEmitente = nota.Informacoes.Emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.Informacoes.identificacao.Numero;
            SerieNota = nota.Informacoes.identificacao.Serie;
            Status = nota.Signature != null && nota.Signature != null ? (int)StatusNota.Assinada : (int)StatusNota.Salva;
            IsNFCe = nota.Informacoes.identificacao.Modelo == 65;
            XML = xml;
        }

        public NFeDI(ProcessoNFe nota, string xml)
        {
            Id = nota.NFe.Informacoes.Id;
            NomeCliente = nota.NFe.Informacoes.destinatário.Nome;
            NomeEmitente = nota.NFe.Informacoes.Emitente.Nome;
            CNPJEmitente = nota.NFe.Informacoes.Emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.NFe.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.NFe.Informacoes.identificacao.Numero;
            SerieNota = nota.NFe.Informacoes.identificacao.Serie;
            Status = nota.ProtNFe != null ? (int)StatusNota.Emitida : nota.NFe.Signature != null ? (int)StatusNota.Assinada : (int)StatusNota.Salva;
            IsNFCe = nota.NFe.Informacoes.identificacao.Modelo == 65;
            XML = xml;
        }

        public NFeDI(ProcessoNFCe nota, string xml)
        {
            Id = nota.NFe.Informacoes.Id;
            NomeCliente = nota.NFe.Informacoes.destinatário.Nome;
            NomeEmitente = nota.NFe.Informacoes.Emitente.Nome;
            CNPJEmitente = nota.NFe.Informacoes.Emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.NFe.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.NFe.Informacoes.identificacao.Numero;
            SerieNota = nota.NFe.Informacoes.identificacao.Serie;
            Status = nota.ProtNFe != null ? (int)StatusNota.Emitida : nota.NFe.Signature != null ? (int)StatusNota.Assinada : (int)StatusNota.Salva;
            IsNFCe = nota.NFe.Informacoes.identificacao.Modelo == 65;
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
