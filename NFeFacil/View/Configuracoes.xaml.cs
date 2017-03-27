using NFeFacil.View.Controles;
using NFeFacil.ViewModel.Configuracoes;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Configuracoes : Page, IEsconde
    {
        public Configuracoes()
        {
            InitializeComponent();
            Propriedades.Intercambio.SeAtualizar(Telas.Configurações, Symbol.Setting, nameof(Configuracoes));
        }

        public async Task Esconder()
        {
            OcultarGrid.Begin();
            await Task.Delay(250);
        }
    }

    public sealed class ExibicaoQR : StateTriggerBase
    {
        public bool Visivel { get; set; }

        private ViewModel.Configuracoes.Sincronizacao contexto;
        public ViewModel.Configuracoes.Sincronizacao Contexto
        {
            get => contexto;
            set
            {
                contexto = value;
                contexto.MostrarQRChanged += Contexto_MostrarQRChanged;
            }
        }

        private void Contexto_MostrarQRChanged(ViewModel.Configuracoes.Sincronizacao sender, ViewModel.Configuracoes.Sincronizacao.MostrarQRChangeEventArgs args)
        {
            SetActive(Visivel == args.DadoAtual);
        }
    }
}
