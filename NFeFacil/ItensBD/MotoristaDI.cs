using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;

namespace NFeFacil.ItensBD
{
    public sealed class MotoristaDI : Motorista
    {
        public int Id { get; set; }
        public MotoristaDI() { }
        public MotoristaDI(Motorista mot) : base(mot) { }
    }
}
