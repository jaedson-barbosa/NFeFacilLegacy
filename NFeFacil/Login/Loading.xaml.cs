using NFeFacil.Sincronizacao;
using NFeFacil.View;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Loading : Page
    {
        EtapaProcesso[] Etapas { get; }

        public Loading()
        {
            InitializeComponent();
            Etapas = new EtapaProcesso[]
            {
                new EtapaProcesso("Processar dados do IBGE"),
                new EtapaProcesso("Analisar banco de dados"),
                new EtapaProcesso("Ajustar background"),
                new EtapaProcesso("Verificar início do servidor"),
                new EtapaProcesso("Adicionar evento de retorno"),
                new EtapaProcesso("Ajustar definições de globalização")
            };
            Start();
        }

        async Task Update(int etapasConcluidas)
        {
            barGeral.Value = etapasConcluidas;
            if (etapasConcluidas > 0)
            {
                Etapas[etapasConcluidas - 1].Atual = EtapaProcesso.Status.Concluido;
                Etapas[etapasConcluidas - 1].Update();
            }
            if (etapasConcluidas < Etapas.Length)
            {
                Etapas[etapasConcluidas].Atual = EtapaProcesso.Status.EmAndamento;
                Etapas[etapasConcluidas].Update();
            }
            await Task.Delay(100);
        }

        async void Start()
        {
            try
            {
                await Update(0);
                ProcessarIBGE();
                await Update(1);
                await AnalisarBanco();
                await Update(2);
                AjustarBackground();
                await Update(3);
                VerificarInicioServidor();
                await Update(4);
                AdicionarEventoRetorno();
                await Update(5);
                AjustarGlobalizacao();
                await Update(6);
                Finalizar();
            }
            catch (Exception e)
            {
                txtAtual.Text = e.Message;
            }
        }

        void ProcessarIBGE()
        {
            IBGE.Estados.Buscar();
            IBGE.Municipios.Buscar();
            DadosEstadosParaView.Iniciar();
        }

        async Task AnalisarBanco()
        {
            using (var analise = new Repositorio.OperacoesExtras())
            {
                await analise.AnalisarBanco(DefinicoesTemporarias.DateTimeNow);
            }
        }

        void AjustarBackground()
        {
            MainPage current = MainPage.Current;
            switch (DefinicoesPermanentes.TipoBackground)
            {
                case TiposBackground.Imagem:
                    if (DefinicoesPermanentes.IDBackgroung != default(Guid))
                    {
                        using (var repo = new Repositorio.Leitura())
                        {
                            var img = repo.ProcurarImagem(DefinicoesPermanentes.IDBackgroung);
                            current.ImagemBackground = img?.Bytes?.GetSource();
                        }
                    }
                    current.DefinirTipoBackground(TiposBackground.Imagem);
                    current.DefinirOpacidadeBackground(DefinicoesPermanentes.OpacidadeBackground);
                    break;
                case TiposBackground.Cor:
                    current.DefinirTipoBackground(TiposBackground.Cor);
                    current.DefinirOpacidadeBackground(DefinicoesPermanentes.OpacidadeBackground);
                    break;
            }
        }

        void AdicionarEventoRetorno()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += (x, e) =>
            {
                e.Handled = true;
                MainPage.Current.Retornar();
            };
        }

        void AjustarGlobalizacao()
        {
            var info = new CultureInfo("pt-BR");
            CultureInfo.CurrentCulture = info;
            CultureInfo.CurrentUICulture = info;
            CultureInfo.DefaultThreadCurrentCulture = info;
            CultureInfo.DefaultThreadCurrentUICulture = info;
        }

        void VerificarInicioServidor()
        {
            if (ConfiguracoesSincronizacao.InícioAutomático)
            {
                GerenciadorServidor.Current.IniciarServer().ConfigureAwait(false);
            }
        }

        async void Finalizar()
        {
            txtAtual.Text = "Sistemas carregados. E obrigado pelo apoio 😃";
            await Task.Delay(1000);

            MainPage current = MainPage.Current;
            using (var repo = new Repositorio.Leitura())
            {
                if (repo.EmitentesCadastrados)
                {
                    current.Navegar<EscolhaEmitente>();
                }
                else
                {
                    current.Navegar<PrimeiroUso>();
                }
            }
        }
    }
}
