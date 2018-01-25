using NFeFacil.ItensBD;
using NFeFacil.View;
using System;
using Windows.UI.Xaml;
using NFeFacil.WebService.Pacotes;
using NFeFacil.WebService.Pacotes.PartesInutNFe;
using Windows.UI.Xaml.Controls;
using System.Xml.Linq;
using Windows.UI.Xaml.Data;
using System.Linq;
using NFeFacil.WebService;
using System.Collections.ObjectModel;
using NFeFacil.Certificacao;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Inutilizacoes : Page
    {
        ObservableCollection<IGrouping<string, Inutilizacao>> Lista { get; }
        ICollectionView Itens { get; }

        public Inutilizacoes()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                Lista = (from imp in repo.ObterInutilizacoes()
                         group imp by imp.Homologacao ? "Homologação" : "Produção").GerarObs();
                Itens = new CollectionViewSource()
                {
                    IsSourceGrouped = true,
                    Source = Lista
                }.View;
            }
        }

        async void AdicionarInutilizacao(object sender, RoutedEventArgs e)
        {
            var caixa = new InfoInutilizacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var envio = new InutNFe(new InfInut(caixa.Homologacao, caixa.Serie, caixa.InicioNum, caixa.FimNum, caixa.Justificativa));

                var uf = DefinicoesTemporarias.EmitenteAtivo.SiglaUF;
                var gerenciador = new GerenciadorGeral<InutNFe, RetInutNFe>(uf, Operacoes.Inutilizacao, caixa.Homologacao);

                AssinaFacil assinador = new AssinaFacil();
                await assinador.Preparar();
                Progresso progresso = null;
                progresso = new Progresso(async x =>
                {
                    var resultAssina = await envio.PrepararEvento(assinador, x);
                    if (!resultAssina.Item1)
                    {
                        return resultAssina;
                    }
                    await progresso.Update(1);

                    var resultado = await gerenciador.EnviarAsync(envio);
                    var sucesso = resultado.Info.StatusResposta == 102;
                    if (sucesso)
                    {
                        Concluir(resultado, caixa.Homologacao);
                        await progresso.Update(6);
                    }
                    return (sucesso, resultado.Info.DescricaoResposta);
                }, assinador.CertificadosDisponiveis, "Subject",
                "Assinar documentos para envio",
                "Preparar conexão",
                "Obter conteúdo da requisição",
                "Enviar requisição",
                "Processar resposta",
                "Salvar retorno no banco de dados");
                gerenciador.ProgressChanged += async (x, y) => await progresso.Update(y + 1);
                await progresso.ShowAsync();
            }
        }

        void Concluir(RetInutNFe resultado, bool homologacao)
        {
            var info = resultado.Info;
            var xml = resultado.ToXElement();
            var itemDB = new Inutilizacao
            {
                CNPJ = info.CNPJ,
                FimRange = info.FinalNumeracao,
                Homologacao = homologacao,
                Id = info.Id,
                InicioRange = info.InicioNumeracao,
                MomentoProcessamento = DateTime.Parse(info.DataHoraProcessamento),
                NumeroProtocolo = info.NumeroProtocolo,
                Serie = info.SerieNFe,
                XMLCompleto = xml.ToString(SaveOptions.DisableFormatting)
            };
            using (var db = new Repositorio.Escrita())
            {
                db.SalvarItemSimples(itemDB, DefinicoesTemporarias.DateTimeNow);
            }

            string key = homologacao ? "Homologação" : "Produção";
            int index = Lista[0].Key == key ? 0 : 1;
            var nova = Lista[index].Concat(new Inutilizacao[1] { itemDB }).GroupBy(x => key);
            Lista.RemoveAt(index);
            Lista.Insert(0, nova.Single());
        }
    }
}
