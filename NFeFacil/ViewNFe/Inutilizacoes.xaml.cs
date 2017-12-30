using NFeFacil.ItensBD;
using NFeFacil.ViewNFe.PartesInutilizacoes;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using NFeFacil.WebService.Pacotes;
using NFeFacil.WebService.Pacotes.PartesInutNFe;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Inutilizacoes : Page
    {
        ObservableCollection<Inutilizacao> Itens { get; }

        public Inutilizacoes()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                Itens = repo.ObterInutilizacoes().GerarObs();
            }
        }

        async void AdicionarInutilizacao(object sender, RoutedEventArgs e)
        {
            var caixa = new InfoInutilizacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var info = new InfInut(caixa.Homologacao, caixa.Serie, caixa.InicioNum, caixa.FimNum, caixa.Justificativa);
                var envio = new InutNFe(info);
                var progresso = new Progresso(envio, caixa.Homologacao);
                await progresso.ShowAsync();
                if (progresso.Sucesso)
                {

                }
            }
        }
    }
}
