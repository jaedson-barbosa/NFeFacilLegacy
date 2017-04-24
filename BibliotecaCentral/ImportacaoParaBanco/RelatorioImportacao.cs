using System.Collections.Generic;

namespace BibliotecaCentral.ImportacaoParaBanco
{
    public class RelatorioImportacao
    {
        public ResumoRelatorioImportacao Analise
        {
            get => Erros.Count == 0 ? ResumoRelatorioImportacao.Sucesso : ResumoRelatorioImportacao.Erro;
        }

        public List<XmlNaoReconhecido> Erros { get; set; } = new List<XmlNaoReconhecido>();
    }
}
