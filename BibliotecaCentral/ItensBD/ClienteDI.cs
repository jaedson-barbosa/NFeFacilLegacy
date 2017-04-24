using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace BibliotecaCentral.ItensBD
{
    public sealed class ClienteDI : Destinatario, IId, IConverterDI<Destinatario>
    {
        public int Id { get; set; }
        public ClienteDI() { }
        public ClienteDI(Destinatario dest) : base(dest) { }

        public IId Converter(Destinatario item) => new ClienteDI(item);
    }
}
