using NFeFacil.Controles;
using NFeFacil.WebService;
using NFeFacil.WebService.Pacotes;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        ObservableCollection<EtapaProcesso> Etapas { get; }

        double ProgressoAtual
        {
            get => barGeral.Value;
            set => barGeral.Value = value;
        }

        public Progresso(InutNFe envio, bool homologacao)
        {
            InitializeComponent();
            Envio = envio;
            Homologacao = homologacao;
            Inutilizar();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            Inutilizar();
        }

        public void Update(int index, EtapaProcesso novo)
        {
            Etapas.RemoveAt(index);
            Etapas.Insert(index, novo);
        }

        async void Inutilizar()
        {
            IsPrimaryButtonEnabled = false;
            IsSecondaryButtonEnabled = false;

            var uf = DefinicoesTemporarias.EmitenteAtivo.SiglaUF;
            var gerenciador = new GerenciadorGeral<InutNFe, RetInutNFe>(uf, Operacoes.Inutilizacao, Homologacao);
            var result = await gerenciador.EnviarAsync(Envio);
            var info = result.Info;
            if (info.StatusResposta == 102)
            {
                Sucesso = true;
                using (var db = new Repositorio.Escrita())
                {
                    var itemDB = new ItensBD.Inutilizacao
                    {
                        CNPJ = info.CNPJ,
                        FimRange = info.FinalNumeracao,
                        Homologacao = Homologacao,
                        Id = info.Id,
                        InicioRange = info.InicioNumeracao,
                        MomentoProcessamento = DateTime.Parse(info.DataHoraProcessamento),
                        NumeroProtocolo = info.NumeroProtocolo,
                        Serie = info.SerieNFe,
                        XMLCompleto = result.ToXElement<RetInutNFe>().ToString(SaveOptions.DisableFormatting)
                    };
                    db.SalvarItemSimples(itemDB, DefinicoesTemporarias.DateTimeNow);
                }
            }
            else
            {
                Sucesso = false;
            }
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

    public struct EtapaProcesso
    {
        public Visibility Concluido { get; set; }
        public Visibility Pendente { get; set; }
        public bool EmAndamento { get; set; }
        public string Descricao { get; set; }
    }
}
