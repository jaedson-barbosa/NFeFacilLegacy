namespace NFeFacil.ModeloXML
{
    public enum TiposDocumento
    {
        CPF,
        CNPJ,
        idEstrangeiro
    }

    public enum ModalidadesTransporte
    {
        Emitente = 0,
        Destinatário_Remetente = 1,
        Terceiros = 2,
        Inexistente = 9
    }
}
