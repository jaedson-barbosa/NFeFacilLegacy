using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace NFeFacil.ItensBD
{
    public sealed class ClienteDI : Destinatario
    {
        public int Id { get; set; }
        public ClienteDI() { }
        public ClienteDI(Destinatario dest) : base(dest) { }
    }
}
