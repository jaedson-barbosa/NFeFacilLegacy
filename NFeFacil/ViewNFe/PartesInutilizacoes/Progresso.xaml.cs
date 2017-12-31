using NFeFacil.Log;
using NFeFacil.WebService;
using NFeFacil.WebService.Pacotes;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.PartesInutilizacoes
{
    public sealed partial class Progresso : ContentDialog
    {
        InutNFe Envio { get; }
        bool Homologacao { get; }
        public bool Sucesso { get; private set; }
        public RetInutNFe Resultado { get; private set; }

        GerenciadorGeral<InutNFe, RetInutNFe> Gerenciador { get; }

        ObservableCollection<EtapaProcesso> Etapas { get; }
        int EtapasExtras { get; }

        public Progresso(InutNFe envio, bool homologacao, params EtapaProcesso[] etapasAdicionais)
        {
            InitializeComponent();
            Envio = envio;
            Homologacao = homologacao;

            var uf = DefinicoesTemporarias.EmitenteAtivo.SiglaUF;
            Gerenciador = new GerenciadorGeral<InutNFe, RetInutNFe>(uf, Operacoes.Inutilizacao, Homologacao);

            Etapas = Gerenciador.Etapas.Select(x => new EtapaProcesso(x)).Concat(etapasAdicionais).GerarObs();
            EtapasExtras = etapasAdicionais.Length;
            Gerenciador.ProgressChanged += Gerenciador_ProgressChanged;

            Inutilizar();
        }

        async Task Gerenciador_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var at = e.EtapasConcluidas;
            var tot = e.TotalEtapas + EtapasExtras;
            barGeral.Value = at * 100 / tot;
            Etapas[at - 1].Atual = EtapaProcesso.Status.Concluido;
            Update(at - 1);
            if (at < tot)
            {
                Etapas[at].Atual = EtapaProcesso.Status.EmAndamento;
                Update(at);
            }
            await Task.Delay(500);
        }

        void Update(int index)
        {
            var item = Etapas[index];
            Etapas.RemoveAt(index);
            Etapas.Insert(index, item);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            Inutilizar();
        }

        void Reset()
        {
            barGeral.Value = 0;
            barGeral.ShowError = false;
            for (int i = 0; i < Etapas.Count; i++)
            {
                Etapas[i].Atual = EtapaProcesso.Status.Pendente;
                Update(i);
            }
        }

        async void Inutilizar()
        {
            Reset();
            IsPrimaryButtonEnabled = false;
            IsSecondaryButtonEnabled = false;

            try
            {
                Resultado = await Gerenciador.EnviarAsync(Envio);
                if (Resultado.Info.StatusResposta == 102)
                {
                    Sucesso = true;
                }
                else
                {
                    barGeral.ShowError = true;
                    Popup.Current.Escrever(TitulosComuns.Atenção, $"A mensagem de retorno é: {Resultado.Info.DescricaoResposta}");
                    Sucesso = false;
                }
            }
            catch (Exception e)
            {
                barGeral.ShowError = true;
                e.ManipularErro();
                Sucesso = false;
            }
            AtualizarBotoes();
        }

        void AtualizarBotoes()
        {
            if (Sucesso)
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
        }
    }

    public sealed class EtapaProcesso
    {
        public EtapaProcesso(string descricao)
        {
            Descricao = descricao;
            Atual = Status.Pendente;
        }

        public Visibility Concluido => Atual == Status.Concluido ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Pendente => Atual == Status.Pendente ? Visibility.Visible : Visibility.Collapsed;
        public bool EmAndamento => Atual == Status.EmAndamento;
        public string Descricao { get; set; }

        internal Status Atual { get; set; }

        public enum Status { Pendente, EmAndamento, Concluido }
    }
}
