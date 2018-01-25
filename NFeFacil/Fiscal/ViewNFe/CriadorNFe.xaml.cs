using NFeFacil.IBGE;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.ModeloXML.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal.ViewNFe
{
    public sealed partial class CriadorNFe : ContentDialog
    {
        bool ambienteHomolocagao;
        bool AmbienteHomolocagao
        {
            get => ambienteHomolocagao;
            set
            {
                ambienteHomolocagao = value;
                if (DefinicoesPermanentes.CalcularNumeroNFe)
                {
                    CalcularNumero_Click(null, null);
                }
            }
        }
        ushort Serie { get; set; } = 1;
        NFe PreNota { get; }

        public CriadorNFe()
        {
            InitializeComponent();
            PreNota = new NFe()
            {
                Informacoes = new InformacoesNFe()
                {
                    Emitente = DefinicoesTemporarias.EmitenteAtivo.ToEmitente(),
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
            if (DefinicoesPermanentes.CalcularNumeroNFe)
            {
                CalcularNumero_Click(null, null);
            }
        }

        public CriadorNFe(NFe preNota)
        {
            InitializeComponent();
            PreNota = preNota;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var identificacao = new Identificacao()
            {
                Serie = Serie,
                Numero = (int)txtNumero.Number,
                TipoAmbiente = (ushort)(AmbienteHomolocagao ? 2 : 1),
                CódigoUF = Estados.Buscar(DefinicoesTemporarias.EmitenteAtivo.SiglaUF).Codigo,
                CodigoMunicipio = DefinicoesTemporarias.EmitenteAtivo.CodigoMunicipio
            };
            identificacao.DefinirVersãoAplicativo();
            PreNota.Informacoes.identificacao = identificacao;
            PreNota.Informacoes.ChaveAcesso = null;
            MainPage.Current.Navegar<ManipulacaoNotaFiscal>(PreNota);
        }

        private void CalcularNumero_Click(object sender, RoutedEventArgs e)
        {
            var cnpj = DefinicoesTemporarias.EmitenteAtivo.CNPJ;
            using (var repo = new Repositorio.Leitura())
            {
                txtNumero.Number = repo.ObterMaiorNumeroNFe(cnpj, Serie, AmbienteHomolocagao) + 1;
            }
        }
    }
}
