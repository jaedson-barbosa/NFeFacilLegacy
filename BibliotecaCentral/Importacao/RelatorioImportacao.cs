using System;
using System.Collections.Generic;

namespace BibliotecaCentral.Importacao
{
    public class RelatorioImportacao
    {
        public ResumoRelatorioImportacao Analise
        {
            get => Erros.Count == 0 ? ResumoRelatorioImportacao.Sucesso : ResumoRelatorioImportacao.Erro;
        }

        public List<Exception> Erros { get; set; } = new List<Exception>();
    }
}
