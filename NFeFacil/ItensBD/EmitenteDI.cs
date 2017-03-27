using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace NFeFacil.ItensBD
{
    public sealed class EmitenteDI : Emitente, IId
    {
        public int Id { get; set; }
        public EmitenteDI() { }
        public EmitenteDI(Emitente emit) : base(emit) { }
    }
}
