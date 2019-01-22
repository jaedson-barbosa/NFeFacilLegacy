using System;
using System.Collections;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace BaseGeral.View
{
    public sealed partial class Progresso : ContentDialog
    {
        EtapaProcesso[] Etapas { get; }
        int TotalEtapas { get; }
        Func<Task<(bool, string)>> Acao { get; }
        object ItemEscolhido { get; set; }

        private Progresso(string[] conjuntoEtapas)
        {
            InitializeComponent();
            Etapas = new EtapaProcesso[conjuntoEtapas.Length];
            for (int i = 0; i < conjuntoEtapas.Length; i++) Etapas[i] = new EtapaProcesso(conjuntoEtapas[i]);
            TotalEtapas = conjuntoEtapas.Length;
        }

        public Progresso(Func<Task<(bool, string)>> acao, params string[] extras)
            : this(extras)
        {
            cmbEscolha.Visibility = Visibility.Collapsed;
            Acao = acao;
        }

        public Progresso(Func<object, Task<(bool, string)>> acao, IEnumerable escolhaItens, string displayPath, params string[] extras)
            : this(extras)
        {
            cmbEscolha.ItemsSource = escolhaItens;
            cmbEscolha.DisplayMemberPath = displayPath;
            Acao = () => acao(ItemEscolhido);
            SecondaryButtonText = "Iniciar";
            IsSecondaryButtonEnabled = true;
        }

        void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            Start();
        }

        public async Task Update(int EtapasConcluidas)
        {
            var at = EtapasConcluidas;
            var tot = TotalEtapas;
            barGeral.Value = at * 100 / tot;
            Etapas[at - 1].Atual = EtapaProcesso.Status.Concluido;
            Etapas[at - 1].Update();
            if (at < tot)
            {
                Etapas[at].Atual = EtapaProcesso.Status.EmAndamento;
                Etapas[at].Update();
            }
            await Task.Delay(250);
        }

        public async void Start()
        {
            barGeral.Value = 0;
            barGeral.ShowError = false;
            for (int i = 0; i < Etapas.Length; i++)
            {
                Etapas[i].Atual = EtapaProcesso.Status.Pendente;
                Etapas[i].Update();
            }

            IsPrimaryButtonEnabled = false;
            IsSecondaryButtonEnabled = false;
            txtResultado.Text = string.Empty;

            try
            {
                var result = await Acao();
                Stop(result.Item1, result.Item2);
            }
            catch (ErroDesserializacao e)
            {
                e.ExportarXML();
                Stop(false, e.Message);
            }
            catch (Exception erro)
            {
                Stop(false, $"{erro.Message}\r\n" +
                    $"Detalhes adicionais: {erro.InnerException?.Message ?? "Não há detalhes"}");
            }
        }

        void Stop(bool sucesso, string mensagem)
        {
            if (sucesso)
            {
                PrimaryButtonText = "Concluir";
                IsSecondaryButtonEnabled = false;
            }
            else
            {
                PrimaryButtonText = "Cancelar";
                IsSecondaryButtonEnabled = true;
            }
            IsPrimaryButtonEnabled = true;
            SecondaryButtonText = "Tentar novamente";
            txtResultado.Text = mensagem;
        }
    }
}
