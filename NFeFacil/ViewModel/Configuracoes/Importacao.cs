using NFeFacil.ImportacaoParaBanco;
using NFeFacil.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NFeFacil.ViewModel.Configuracoes
{
    public sealed class Importacao
    {
        private readonly ILog LogPopUp = new Popup();

        public ICommand ImportarNotaFiscalCommand { get; }
        public ICommand ImportarDadoBaseCommand { get; }

        public async void ImportarNotaFiscal()
        {
            var import = new ImportarNotaFiscal();
            await import.Importar();
            LogPopUp.Escrever(TitulosComuns.Sucesso, "As notas fiscais foram importadas com sucesso.");
        }

        public async void ImportarDadoBase()
        {
            var import = new ImportarDadoBase(TipoBásicoSelecionado);
            await import.Importar();
            LogPopUp.Escrever(TitulosComuns.Sucesso, "As informações base foram importadas com sucesso.");
        }

        public IEnumerable<TiposDadoBásico> TiposBásicos
        {
            get
            {
                return Enum.GetValues(typeof(TiposDadoBásico)).Cast<TiposDadoBásico>();
            }
        }

        public TiposDadoBásico TipoBásicoSelecionado { get; set; }

        public Importacao()
        {
            ImportarNotaFiscalCommand = new ComandoSemParametros(ImportarNotaFiscal, true);
            ImportarDadoBaseCommand = new ComandoSemParametros(ImportarDadoBase, true);
        }
    }
}
