using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.ItensBD
{
    public sealed class NFeDI
    {
        public string Id { get; set; }
        [Required]
        public string NumeroNota { get; set; }
        [Required]
        public string NomeEmitente { get; set; }
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
                var nota = xml.FromXElement<NFe>();
                return new NFeDI
                {
                    Id = nota.Informações.Id,
                    NomeCliente = nota.Informações.destinatário.nome,
                    NomeEmitente = nota.Informações.emitente.nome,
                    DataEmissao = nota.Informações.identificação.DataHoraEmissão,
                    NumeroNota = nota.Informações.identificação.Numero.ToString(),
                    Status = nota.Signature != null && nota.Signature.HasElements ? (int)StatusNFe.Assinado : (int)StatusNFe.Salvo
                };
            }
            else
            {
                var nota = xml.FromXElement<Processo>();
                return new NFeDI
                {
                    Id = nota.NFe.Informações.Id,
                    NomeCliente = nota.NFe.Informações.destinatário.nome,
                    NomeEmitente = nota.NFe.Informações.emitente.nome,
                    DataEmissao = nota.NFe.Informações.identificação.DataHoraEmissão,
                    NumeroNota = nota.NFe.Informações.identificação.Numero.ToString(),
                    Status = nota.ProtNFe != null ? (int)StatusNFe.Emitido : nota.NFe.Signature != null && nota.NFe.Signature.HasElements ? (int)StatusNFe.Assinado : (int)StatusNFe.Salvo
                };
            }
        }

        public async Task<object> ConjuntoCompletoAsync()
        {
            var pasta = new PastaNotasFiscais();
            if (Status < 4)
                return await pasta.Retornar<NFe>(Id);
            else
                return await pasta.Retornar<Processo>(Id);
        }
    }
}
