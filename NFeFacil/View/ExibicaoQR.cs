using NFeFacil.ViewModel;
using Windows.UI.Xaml;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    public sealed class ExibicaoQR : StateTriggerBase
    {
        public bool Visivel { get; set; }

        private ConfigSincronizacaoDataContext contexto;
        public ConfigSincronizacaoDataContext Contexto
        {
            get => contexto;
            set
            {
                contexto = value;
                contexto.MostrarQRChanged += Contexto_MostrarQRChanged;
            }
        }

        private void Contexto_MostrarQRChanged(ConfigSincronizacaoDataContext sender, ConfigSincronizacaoDataContext.MostrarQRChangeEventArgs args)
        {
            SetActive(Visivel == args.DadoAtual);
        }
    }
}
