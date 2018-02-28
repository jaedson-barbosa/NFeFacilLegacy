using NFeFacil.ModeloXML;
using System;
using System.ComponentModel.DataAnnotations;

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

        public NFeDI() { }

        public NFeDI(NFe nota, string xml)
        {
            if (nota.Informacoes.identificacao.Modelo == 65)
                throw new Exception();

            Id = nota.Informacoes.Id;
            NomeCliente = nota.Informacoes.destinatário.Nome;
            NomeEmitente = nota.Informacoes.Emitente.Nome;
            CNPJEmitente = nota.Informacoes.Emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.Informacoes.identificacao.Numero;
            SerieNota = nota.Informacoes.identificacao.Serie;
            Status = nota.Signature != null && nota.Signature != null ? (int)StatusNota.Assinada : (int)StatusNota.Salva;
            IsNFCe = false;
            XML = xml;
        }

        public NFeDI(NFCe nota, string xml)
        {
            if (nota.Informacoes.identificacao.Modelo == 55)
                throw new Exception();

            Id = nota.Informacoes.Id;
            NomeCliente = nota.Informacoes.destinatário?.Nome ?? "Desconhecido";
            NomeEmitente = nota.Informacoes.Emitente.Nome;
            CNPJEmitente = nota.Informacoes.Emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.Informacoes.identificacao.Numero;
            SerieNota = nota.Informacoes.identificacao.Serie;
            Status = nota.Signature != null && nota.Signature != null ? (int)StatusNota.Assinada : (int)StatusNota.Salva;
            IsNFCe = true;
            XML = xml;
        }

        public NFeDI(ProcessoNFe nota, string xml)
        {
            if (nota.NFe.Informacoes.identificacao.Modelo == 65)
                throw new Exception();

            Id = nota.NFe.Informacoes.Id;
            NomeCliente = nota.NFe.Informacoes.destinatário.Nome;
            NomeEmitente = nota.NFe.Informacoes.Emitente.Nome;
            CNPJEmitente = nota.NFe.Informacoes.Emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.NFe.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.NFe.Informacoes.identificacao.Numero;
            SerieNota = nota.NFe.Informacoes.identificacao.Serie;
            Status = nota.ProtNFe != null ? (int)StatusNota.Emitida : nota.NFe.Signature != null ? (int)StatusNota.Assinada : (int)StatusNota.Salva;
            IsNFCe = false;
            XML = xml;
        }

        public NFeDI(ProcessoNFCe nota, string xml)
        {
            if (nota.NFe.Informacoes.identificacao.Modelo == 55)
                throw new Exception();

            Id = nota.NFe.Informacoes.Id;
            NomeCliente = nota.NFe.Informacoes.destinatário?.Nome ?? "Desconhecido";
            NomeEmitente = nota.NFe.Informacoes.Emitente.Nome;
            CNPJEmitente = nota.NFe.Informacoes.Emitente.CNPJ.ToString();
            DataEmissao = DateTime.Parse(nota.NFe.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
            NumeroNota = nota.NFe.Informacoes.identificacao.Numero;
            SerieNota = nota.NFe.Informacoes.identificacao.Serie;
            Status = nota.ProtNFe != null ? (int)StatusNota.Emitida : nota.NFe.Signature != null ? (int)StatusNota.Assinada : (int)StatusNota.Salva;
            IsNFCe = true;
            XML = xml;
        }
    }
}
