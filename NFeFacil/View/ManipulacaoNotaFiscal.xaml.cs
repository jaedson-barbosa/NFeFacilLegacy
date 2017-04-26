using BibliotecaCentral.ItensBD;
using NFeFacil.ViewModel;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoNotaFiscal : Page, IEsconde, IValida
    {
        private NotaFiscalDataContext contexto;

        public ManipulacaoNotaFiscal()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = (ConjuntoManipuladorNFe)e.Parameter;
            switch (param.OperacaoRequirida)
            {
                case TipoOperacao.Adicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.ManipularNota, Symbol.Add, "Criar nota fiscal");
                    break;
                case TipoOperacao.Edicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.ManipularNota, Symbol.Edit, "Editar nota fiscal");
                    break;
                default:
                    break;
            }
            DataContext = contexto = new NotaFiscalDataContext(ref param);
        }

        async Task<bool> IValida.Verificar()
        {
            var retorno = true;
            if (contexto.StatusAtual == StatusNFe.EdiçãoCriação)
            {
                var mensagem = new MessageDialog("Se você sair agora, os dados serão perdidos, se tiver certeza, escolha Sair, caso contrário, Cancelar.", "Atenção");
                mensagem.Commands.Add(new UICommand("Sair"));
                mensagem.Commands.Add(new UICommand("Cancelar", x => retorno = false));
                await mensagem.ShowAsync();
            }
            return retorno;
        }

        async Task IEsconde.EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }
    }
}
