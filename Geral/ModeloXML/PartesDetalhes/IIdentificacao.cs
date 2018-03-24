namespace NFeFacil.ModeloXML.PartesDetalhes
{
    public interface IIdentificacao
    {
        int ChaveNF { get; set; }
        string DataHoraEmissão { get; set; }
        int DígitoVerificador { get; set; }
        ushort Modelo { get; set; }
        int Numero { get; set; }
        ushort Serie { get; set; }
        ushort TipoEmissão { get; set; }
    }
}