using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace BibliotecaCentral.ItensBD
{
    public sealed class EmitenteDI : Emitente, IId, IConverterDI<Emitente>
    {
        public int Id { get; set; }
        public EmitenteDI() { }
        public EmitenteDI(Emitente emit) : base(emit) { }

        public IId Converter(Emitente item) => new EmitenteDI(item);
    }
}
