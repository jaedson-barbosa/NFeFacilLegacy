using NFeFacil.ItensBD;
using NFeFacil.ViewNFe.PartesInutilizacoes;
using System;
using Windows.UI.Xaml;
using NFeFacil.WebService.Pacotes;
using NFeFacil.WebService.Pacotes.PartesInutNFe;
using Windows.UI.Xaml.Controls;
using System.Xml.Linq;
using Windows.UI.Xaml.Data;
using System.Linq;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Inutilizacoes : Page
    {
        ICollectionView Itens { get; }

        public Inutilizacoes()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                Itens = new CollectionViewSource()
                {
                    IsSourceGrouped = true,
                    Source = from imp in repo.ObterInutilizacoes()
                             group imp by imp.Homologacao ? "Homologação" : "Produção"
                }.View;
            }
        }

        async void AdicionarInutilizacao(object sender, RoutedEventArgs e)
        {
            var caixa = new InfoInutilizacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var envio = new InutNFe(new InfInut(caixa.Homologacao, caixa.Serie, caixa.InicioNum, caixa.FimNum, caixa.Justificativa));
                await envio.PrepararEventos();
                var progresso = new Progresso(envio, caixa.Homologacao);
                await progresso.ShowAsync();
                if (progresso.Sucesso)
                {
                    var info = progresso.Resultado.Info;
                    using (var db = new Repositorio.Escrita())
                    {
                        var itemDB = new Inutilizacao
                        {
                            CNPJ = info.CNPJ,
                            FimRange = info.FinalNumeracao,
                            Homologacao = caixa.Homologacao,
                            Id = info.Id,
                            InicioRange = info.InicioNumeracao,
                            MomentoProcessamento = DateTime.Parse(info.DataHoraProcessamento),
                            NumeroProtocolo = info.NumeroProtocolo,
                            Serie = info.SerieNFe,
                            XMLCompleto = progresso.Resultado.ToXElement<RetInutNFe>()
                                .ToString(SaveOptions.DisableFormatting)
                        };
                        db.SalvarItemSimples(itemDB, DefinicoesTemporarias.DateTimeNow);
                    }
                }
            }
        }
    }
}
