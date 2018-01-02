using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.View;
using NFeFacil.WebService;
using NFeFacil.WebService.Pacotes;
using System;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    [View.DetalhePagina(Symbol.Find, "Consulta")]
    public sealed partial class Consulta : Page
    {
        string Chave { get; set; }
        bool Homologacao { get; set; }
        Estado UF { get; set; }

        public Consulta()
        {
            InitializeComponent();
        }

        private async void Analisar(object sender, RoutedEventArgs e)
        {
            if (UF == null || string.IsNullOrEmpty(Chave))
            {
                return;
            }
            var gerenciador = new GerenciadorGeral<ConsSitNFe, RetConsSitNFe>(UF, Operacoes.Consultar, Homologacao);
            var envio = new ConsSitNFe(Chave, Homologacao);

            RetConsSitNFe resultado = default(RetConsSitNFe);
            Progresso progresso = null;
            progresso = new Progresso(async () =>
            {
                resultado = await gerenciador.EnviarAsync(envio);
                await progresso.Update(5);
                return (true, resultado.DescricaoResposta);
            }, gerenciador.Etapas, "Analisar resultado no banco de dados");
            gerenciador.ProgressChanged += async (x, y) => await progresso.Update(y);
            progresso.Start();
            await progresso.ShowAsync();

            if (resultado.StatusResposta == 100)
            {
                NFeDI nota = null;
                using (var leit = new Repositorio.Leitura())
                {
                    nota = leit.ObterNFe($"NFe{resultado.ChaveNFe}");
                }
                if (nota != null && nota.Status < 4)
                {
                    using (var esc = new Repositorio.Escrita())
                    {
                        nota.Status = (int)StatusNFe.Emitida;
                        var original = XElement.Parse(nota.XML).FromXElement<NFe>();
                        var novo = new Processo()
                        {
                            NFe = original,
                            ProtNFe = resultado.Protocolo
                        };
                        nota.XML = novo.ToXElement<Processo>().ToString();
                        esc.SalvarItemSimples(nota, DefinicoesTemporarias.DateTimeNow);
                    }
                }
            }
        }
    }
}
