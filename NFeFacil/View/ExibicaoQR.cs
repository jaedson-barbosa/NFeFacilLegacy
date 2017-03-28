using Windows.UI.Xaml;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
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
