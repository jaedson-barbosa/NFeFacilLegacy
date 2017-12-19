using NFeFacil.IBGE;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
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
                if (ConfiguracoesPermanentes.CalcularNumeroNFe)
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
                Informacoes = new Detalhes()
                {
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
            if (ConfiguracoesPermanentes.CalcularNumeroNFe)
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
            if (PreNota.Informacoes.identificacao != null)
            {
                var identificacao = PreNota.Informacoes.identificacao;
                identificacao.Serie = Serie;
                identificacao.Numero = (int)txtNumero.Number;
                identificacao.TipoAmbiente = (ushort)(AmbienteHomolocagao ? 2 : 1);
                identificacao.CódigoUF = Estados.Buscar(Propriedades.EmitenteAtivo.SiglaUF).Codigo;
                identificacao.CodigoMunicipio = Propriedades.EmitenteAtivo.CodigoMunicipio;
                identificacao.ChaveNF = default(int);
                identificacao.DefinirVersãoAplicativo();
            }
            else
            {
                var identificacao = new Identificacao()
                {
                    Serie = Serie,
                    Numero = (int)txtNumero.Number,
                    TipoAmbiente = (ushort)(AmbienteHomolocagao ? 2 : 1),
                    CódigoUF = Estados.Buscar(Propriedades.EmitenteAtivo.SiglaUF).Codigo,
                    CodigoMunicipio = Propriedades.EmitenteAtivo.CodigoMunicipio
                };
                identificacao.DefinirVersãoAplicativo();
                PreNota.Informacoes.identificacao = identificacao;
            }
            PreNota.Informacoes.ChaveAcesso = null;
            MainPage.Current.Navegar<ManipulacaoNotaFiscal>(PreNota);
        }

        private void CalcularNumero_Click(object sender, RoutedEventArgs e)
        {
            var cnpj = Propriedades.EmitenteAtivo.CNPJ;
            using (var repo = new Repositorio.Leitura())
            {
                txtNumero.Number = repo.ObterMaiorNumeroNFe(cnpj, Serie, AmbienteHomolocagao) + 1;
            }
        }
    }
}
