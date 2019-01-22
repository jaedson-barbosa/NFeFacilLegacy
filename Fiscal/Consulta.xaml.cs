using BaseGeral;
using BaseGeral.IBGE;
using BaseGeral.ItensBD;
using BaseGeral.ModeloXML;
using BaseGeral.View;
using Fiscal.WebService;
using Fiscal.WebService.Pacotes;
using System;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fiscal
{
    [DetalhePagina(Symbol.Find, "Consulta")]
    public sealed partial class Consulta : Page
    {
        string Chave { get; set; }
        bool Homologacao { get; set; }
        Estado UF { get; set; }
        bool isNFCe;

        public Consulta()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            isNFCe = e.Parameter != null ? (bool)e.Parameter : false;
        }

        private async void Analisar(object sender, RoutedEventArgs e)
        {
            if (UF == null || string.IsNullOrEmpty(Chave))
            {
                return;
            }
            var gerenciador = new GerenciadorGeral<ConsSitNFe, RetConsSitNFe>(UF, Operacoes.Consultar, Homologacao, isNFCe);
            var envio = new ConsSitNFe(Chave, Homologacao);

            RetConsSitNFe resultado = default(RetConsSitNFe);
            Progresso progresso = null;
            progresso = new Progresso(async () =>
            {
                resultado = await gerenciador.EnviarAsync(envio);
                await progresso.Update(5);
                return (true, resultado.DescricaoResposta);
            }, gerenciador.Etapas.Concat("Analisar resultado no banco de dados"));
            gerenciador.ProgressChanged += async (x, y) => await progresso.Update(y);
            progresso.Start();
            await progresso.ShowAsync();

            if (resultado.StatusResposta == 100)
            {
                NFeDI nota = null;
                using (var leit = new BaseGeral.Repositorio.Leitura())
                {
                    nota = leit.ObterNota($"NFe{resultado.ChaveNFe}");
                }
                if (nota != null && nota.Status < 4)
                {
                    using (var esc = new BaseGeral.Repositorio.Escrita())
                    {
                        nota.Status = (int)StatusNota.Emitida;
                        var original = XElement.Parse(nota.XML).FromXElement<NFe>();
                        var novo = new ProcessoNFe()
                        {
                            NFe = original,
                            ProtNFe = resultado.Protocolo
                        };
                        nota.XML = novo.ToXElement().ToString();
                        esc.SalvarItemSimples(nota, DefinicoesTemporarias.DateTimeNow);
                    }
                }
            }
        }
    }
}
