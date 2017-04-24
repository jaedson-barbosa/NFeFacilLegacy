namespace BibliotecaCentral.ImportacaoParaBanco
{
    internal struct XmlNaoReconhecido
    {
        public string NomeArquivo { get; }
        public string TagRaiz { get; }
        public string[] TagsEsperadas { get; }

        public XmlNaoReconhecido(string nomeArquivo, string tagRaiz, params string[] tagsEsperadas)
        {
            NomeArquivo = nomeArquivo;
            TagRaiz = tagRaiz;
            TagsEsperadas = tagsEsperadas;
        }
    }
}
