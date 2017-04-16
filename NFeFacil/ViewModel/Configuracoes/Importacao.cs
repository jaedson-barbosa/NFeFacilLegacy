using NFeFacil.ImportacaoParaBanco;
using NFeFacil.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace NFeFacil.ViewModel.Configuracoes
{
    public sealed class Importacao
    {
        private readonly ILog log = new Popup();

        public ICommand ImportarNotaFiscalCommand { get; }
        public ICommand ImportarDadoBaseCommand { get; }

        public async void ImportarNotaFiscal()
        {
            var resultado = await new ImportarNotaFiscal().Importar();
            if (resultado.Analise == ResumoRelatorioImportacao.Sucesso)
            {
                log.Escrever(TitulosComuns.Sucesso, "As notas fiscais foram importadas com sucesso.");
            }
            else
            {
                StringBuilder stringErros = new StringBuilder();
                stringErros.AppendLine("As seguintes notas fiscais não foram reconhecidas por terem a tag raiz diferente de nfeProc e de NFe.");
                resultado.Erros.ForEach(x => stringErros.AppendLine($"Nome arquivo: {x.NomeArquivo}; Tag raiz: Encontrada: {x.TagRaiz}"));
                log.Escrever(TitulosComuns.ErroSimples, stringErros.ToString());
            }
        }

        public async void ImportarDadoBase()
        {
            var resultado = await new ImportarDadoBase(TipoBásicoSelecionado).Importar();
            if (resultado.Analise == ResumoRelatorioImportacao.Sucesso)
            {
                log.Escrever(TitulosComuns.Sucesso, "As informações base foram importadas com sucesso.");
            }
            else
            {
                StringBuilder stringErros = new StringBuilder();
                stringErros.AppendLine("Os seguintes dados base não foram reconhecidos por terem a tag raiz diferente do esperado.");
                resultado.Erros.ForEach(x => stringErros.AppendLine($"Nome arquivo: {x.NomeArquivo}; Tag raiz encontrada: {x.TagRaiz}; Tags raiz esperadas: {x.TagsEsperadas[0]} ou {x.TagsEsperadas[1]}"));
                log.Escrever(TitulosComuns.ErroSimples, stringErros.ToString());
            }
        }

        public IEnumerable<TiposDadoBasico> TiposBásicos
        {
            get => Enum.GetValues(typeof(TiposDadoBasico)).Cast<TiposDadoBasico>();
        }

        public TiposDadoBasico TipoBásicoSelecionado { get; set; }

        public Importacao()
        {
            ImportarNotaFiscalCommand = new ComandoSimples(ImportarNotaFiscal, true);
            ImportarDadoBaseCommand = new ComandoSimples(ImportarDadoBase, true);
        }
    }
}
