using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe;

namespace NFeFacil.DANFE.Modelos.Global
{
    public sealed class DadosNFe
    {
        public string NomeEmitente;
        public string TipoEmissao;
        public string NumeroNota;
        public string SerieNota;
        public string PaginaAtual;
        public string QuantPaginas;
        public string Chave;
        public string NumeroProtocolo;
        public string DataHoraRecibo;
        public string NatOp;
        public string IE;
        public string CNPJEmit;
        public Endereço Endereco;

        public void DefinirPagina(int totPagina, int paginaAtual)
        {
            QuantPaginas = totPagina.ToString();
            PaginaAtual = paginaAtual.ToString();
        }
    }

    public sealed class Endereço
    {
        public string UF;
        public string Municipio;
        public string CEP;
        public string Bairro;
        public string Logradouro;
        public string Numero;
        public string Fone;

        public static implicit operator Endereço (enderecoCompleto end)
        {
            return new Endereço
            {
                Bairro = end.Bairro,
                CEP = end.CEP,
                Fone = end.Telefone,
                Logradouro = end.Logradouro,
                Municipio = end.NomeMunicipio,
                Numero = end.Numero,
                UF = end.SiglaUF
            };
        }
    }
}
