using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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

        public static NFeDI Converter(XElement xml)
        {
            if (xml.Name.LocalName == nameof(NFe))
            {
                return new NFeDI(xml.FromXElement<NFe>());
            }
            else
            {
                return new NFeDI(xml.FromXElement<Processo>());
            }
        }

        public NFeDI() { }
        public NFeDI(NFe nota)
        {
            Id = nota.Informações.Id;
            NomeCliente = nota.Informações.destinatário.nome;
            NomeEmitente = nota.Informações.emitente.nome;
            CNPJEmitente = nota.Informações.emitente.CNPJ;
            DataEmissao = nota.Informações.identificação.DataHoraEmissão;
            NumeroNota = nota.Informações.identificação.Numero;
            SerieNota = nota.Informações.identificação.Serie;
            Status = nota.Signature != null && nota.Signature.HasChildNodes ? (int)StatusNFe.Assinada : (int)StatusNFe.Salva;
        }
        public NFeDI(Processo nota)
        {
            Id = nota.NFe.Informações.Id;
            NomeCliente = nota.NFe.Informações.destinatário.nome;
            NomeEmitente = nota.NFe.Informações.emitente.nome;
            CNPJEmitente = nota.NFe.Informações.emitente.CNPJ;
            DataEmissao = nota.NFe.Informações.identificação.DataHoraEmissão;
            NumeroNota = nota.NFe.Informações.identificação.Numero;
            SerieNota = nota.NFe.Informações.identificação.Serie;
            Status = nota.ProtNFe != null ? (int)StatusNFe.Emitida : nota.NFe.Signature != null && nota.NFe.Signature.HasChildNodes ? (int)StatusNFe.Assinada : (int)StatusNFe.Salva;
        }

        public async Task<object> ObjetoCompletoAsync()
        {
            var pasta = new PastaNotasFiscais();
            if (Status < 4)
                return await pasta.Retornar<NFe>(Id);
            else
                return await pasta.Retornar<Processo>(Id);
        }
    }
}
