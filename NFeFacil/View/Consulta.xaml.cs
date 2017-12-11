using NFeFacil.IBGE;
using NFeFacil.WebService;
using NFeFacil.WebService.Pacotes;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Consulta : Page
    {
        string Chave { get; set; }
        Estado UF { get; set; }
        ObservableCollection<string> Resultados { get; } = new ObservableCollection<string>();

        public Consulta()
        {
            InitializeComponent();
        }

        private async void Analisar(object sender, RoutedEventArgs e)
        {
            if (UF == null || string.IsNullOrEmpty(Chave))
            {
                Resultados.Insert(0, "Estado e Chave são informações obrigatórias.");
            }
            else
            {
                btnAnalisar.IsEnabled = false;
                carregamento.IsActive = true;
                try
                {
                    var resp = await new GerenciadorGeral<ConsSitNFe, RetConsSitNFe>(UF, Operacoes.Consultar, false)
                        .EnviarAsync(new ConsSitNFe(Chave));
                    Resultados.Insert(0, resp.xMotivo);
                }
                catch (Exception erro)
                {
                    erro.ManipularErro();
                }
                btnAnalisar.IsEnabled = true;
                carregamento.IsActive = false;
            }
        }
    }
}
