using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BibliotecaCentral.ItensBD
{
    public sealed class NFeDI
    {
        public string Id { get; set; }

        public DateTime UltimaData { get; set; }
        [Required]
        public long NumeroNota { get; set; }
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

        public static NFeDI Converter(XElement xml)
        {
            if (xml.Name.LocalName == nameof(NFe))
            {
                return new NFeDI(xml.FromXElement<NFe>(), xml.ToString());
            }
            else
            {
                return new NFeDI(xml.FromXElement<Processo>(), xml.ToString());
            }
        }

        public NFeDI() { }
        public NFeDI(NFe nota, string xml)
        {
            Id = nota.Informações.Id;
            NomeCliente = nota.Informações.destinatário.nome;
            NomeEmitente = nota.Informações.emitente.nome;
            CNPJEmitente = nota.Informações.emitente.CNPJ;
            DataEmissao = DateTime.Parse(nota.Informações.identificação.DataHoraEmissão).ToString("yyyy-MM-dd hh:mm:ss");
            NumeroNota = nota.Informações.identificação.Numero;
            SerieNota = nota.Informações.identificação.Serie;
            Status = nota.Signature != null && nota.Signature.HasChildNodes ? (int)StatusNFe.Assinada : (int)StatusNFe.Salva;
            XML = xml;
        }
        public NFeDI(Processo nota, string xml)
        {
            Id = nota.NFe.Informações.Id;
            NomeCliente = nota.NFe.Informações.destinatário.nome;
            NomeEmitente = nota.NFe.Informações.emitente.nome;
            CNPJEmitente = nota.NFe.Informações.emitente.CNPJ;
            DataEmissao = DateTime.Parse(nota.NFe.Informações.identificação.DataHoraEmissão).ToString("yyyy-MM-dd hh:mm:ss");
            NumeroNota = nota.NFe.Informações.identificação.Numero;
            SerieNota = nota.NFe.Informações.identificação.Serie;
            Status = nota.ProtNFe != null ? (int)StatusNFe.Emitida : nota.NFe.Signature != null && nota.NFe.Signature.HasChildNodes ? (int)StatusNFe.Assinada : (int)StatusNFe.Salva;
            XML = xml;
        }
    }
}
