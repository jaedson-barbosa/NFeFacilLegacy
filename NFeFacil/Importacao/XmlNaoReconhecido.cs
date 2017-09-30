using System;

namespace NFeFacil.Importacao
{
    public sealed class XmlNaoReconhecido : Exception
    {
        public string NomeArquivo { get; }
        public string TagRaiz { get; }
        public string[] TagsEsperadas { get; }

        public XmlNaoReconhecido(string nomeArquivo, string tagRaiz, params string[] tagsEsperadas) : base($"O arquivo {nomeArquivo} não foi reconhecido.")
        {
            NomeArquivo = nomeArquivo;
            TagRaiz = tagRaiz;
            TagsEsperadas = tagsEsperadas;
        }
    }
}
