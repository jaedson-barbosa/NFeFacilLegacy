using BaseGeral;
using BaseGeral.IBGE;
using BaseGeral.Sincronizacao;
using BaseGeral.View;
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
    [DetalhePagina("\uE9F3", "Carregamento inicial")]
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
                new EtapaProcesso("Ajustar definições de globalização"),
                new EtapaProcesso("Analisar compras")
            };
            Start();
        }

        void Update(int etapasConcluidas)
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
        }

        async void Start()
        {
            try
            {
                Update(0);
                ProcessarIBGE();
                Update(1);
                await AnalisarBanco();
                Update(2);
                AjustarBackground();
                Update(3);
                VerificarInicioServidor();
                Update(4);
                AdicionarEventoRetorno();
                Update(5);
                AjustarGlobalizacao();
                Update(6);
                await AnalisarCompras();
                Update(7);
                Finalizar();
            }
            catch (Exception e)
            {
                txtAtual.Text = e.Message;
            }
        }

        void ProcessarIBGE()
        {
            Estados.Buscar();
            Municipios.Buscar();
            DadosEstadosParaView.Iniciar();
        }

        async Task AnalisarBanco()
        {
            using (var analise = new BaseGeral.Repositorio.OperacoesExtras())
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
                        using (var repo = new BaseGeral.Repositorio.Leitura())
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

        async Task AnalisarCompras() => await ComprasInApp.AnalisarCompras();

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
            await Task.Delay(500);

            MainPage current = MainPage.Current;
            using (var repo = new BaseGeral.Repositorio.Leitura())
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
