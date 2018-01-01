using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.PartesInutilizacoes
{
    public sealed partial class Progresso : ContentDialog
    {
        EtapaProcesso[] Etapas { get; }
        int TotalEtapas { get; }
        Func<Task<(bool, string)>> Acao { get; }

        public Progresso(Func<Task<(bool, string)>> acao, EtapaProcesso[] etapas)
        {
            InitializeComponent();
            Etapas = etapas;
            TotalEtapas = Etapas.Length;
            Acao = acao;
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
            await Task.Delay(500);
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
            catch (Exception e)
            {
                Stop(false, e.Message);
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
            txtResultado.Text = mensagem;
        }
    }

    public sealed class EtapaProcesso : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void Update()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Concluido)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pendente)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmAndamento)));
        }

        public enum Status { Pendente, EmAndamento, Concluido }
    }
}
