using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    public sealed partial class CriadorNFe : ContentDialog
    {
        bool AmbienteHomolocagao { get; set; }
        ushort Serie { get; set; } = 1;

        public CriadorNFe()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var notaSimples = new NFe()
            {
                Informacoes = new Detalhes()
                {
                    identificacao = new Identificacao()
                    {
                        Serie = Serie,
                        Numero = (int)txtNumero.Number,
                        TipoAmbiente = (ushort)(AmbienteHomolocagao ? 2 : 1)
                    },
                    emitente = Propriedades.EmitenteAtivo.ToEmitente(),
                    destinatário = new Destinatario(),
                    produtos = new List<DetalhesProdutos>(),
                    transp = new Transporte()
                    {
                        Transporta = new Motorista(),
                        RetTransp = new ICMSTransporte(),
                        VeicTransp = new Veiculo()
                    },
                    cobr = new Cobranca(),
                    infAdic = new InformacoesAdicionais(),
                    exporta = new Exportacao(),
                    compra = new Compra(),
                    cana = new RegistroAquisicaoCana()
                }
            };
            notaSimples.Informacoes.identificacao.DefinirVersãoAplicativo();
            MainPage.Current.Navegar<ManipulacaoNotaFiscal>(notaSimples);
        }

        private void CalcularNumero_Click(object sender, RoutedEventArgs e)
        {
            var cnpj = Propriedades.EmitenteAtivo.CNPJ;
            using (var Contexto = new AplicativoContext())
            {
                txtNumero.Number = (from nota in Contexto.NotasFiscais
                                    where nota.CNPJEmitente == cnpj.ToString()
                                    where nota.SerieNota == Serie
                                    let notaHomologacao = nota.NomeCliente.Trim().ToUpper() == "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL"
                                    where AmbienteHomolocagao ? notaHomologacao : !notaHomologacao
                                    select nota.NumeroNota).Max() + 1;
            }
        }
    }
}
