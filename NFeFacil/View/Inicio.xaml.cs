using NFeFacil.Certificacao;
using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML;
using NFeFacil.Produto.GerenciamentoProdutos;
using NFeFacil.Sincronizacao;
using NFeFacil.Validacao;
using NFeFacil.ViewDadosBase;
using NFeFacil.Fiscal.ViewNFe;
using NFeFacil.ViewRegistroVenda;
using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using NFeFacil.Fiscal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    [DetalhePagina(Symbol.Home, "Início")]
    public sealed partial class Inicio : Page
    {
        public Inicio()
        {
            InitializeComponent();
        }

        void AbrirClientes(object sender, TappedRoutedEventArgs e) => Navegar<GerenciarClientes>();
        void AbrirMotoristas(object sender, TappedRoutedEventArgs e) => Navegar<GerenciarMotoristas>();
        void AbrirProdutos(object sender, TappedRoutedEventArgs e) => Navegar<GerenciarProdutos>();
        void AbrirVendedores(object sender, TappedRoutedEventArgs e) => Navegar<GerenciarVendedores>();
        void AbrirCompradores(object sender, TappedRoutedEventArgs e) => Navegar<GerenciarCompradores>();

#pragma warning disable CS4014
        void CriarNFe(object sender, TappedRoutedEventArgs e)
        {
            var controle = new ControleNFe();
            new Criador(controle).ShowAsync();
        }

        void CriarNFeEntrada(object sender, TappedRoutedEventArgs e) => CriarNFeEntrada();
#pragma warning restore CS4014

        void AbrirInutilizacoes(object sender, TappedRoutedEventArgs e) => Navegar<Inutilizacoes>();
        void AbrirNotasSalvas(object sender, TappedRoutedEventArgs e) => Navegar<NotasSalvas>();
        void AbrirConsulta(object sender, TappedRoutedEventArgs e) => Navegar<Consulta>();
        void AbrirVendasAnuais(object sender, TappedRoutedEventArgs e) => Navegar<VendasAnuais>();

        async void CriarNFCe(object sender, TappedRoutedEventArgs e)
        {
            var controle = new ControleNFCe();
            await new Criador(controle).ShowAsync();
        }
        void AbrirInutilizacoesNFCe(object sender, TappedRoutedEventArgs e) => Navegar<Inutilizacoes>(true);
        void AbrirNFCesSalvas(object sender, TappedRoutedEventArgs e) => Navegar<NotasSalvas>(true);

        void AbrirVendasSalvas(object sender, TappedRoutedEventArgs e) => Navegar<RegistrosVenda>();
        void CriarVenda(object sender, TappedRoutedEventArgs e)
        {
            var rv = new RegistroVenda
            {
                Emitente = DefinicoesTemporarias.EmitenteAtivo.Id,
                Vendedor = DefinicoesTemporarias.VendedorAtivo?.Id ?? Guid.Empty,
                Produtos = new System.Collections.Generic.List<ProdutoSimplesVenda>(),
                DataHoraVenda = DefinicoesTemporarias.DateTimeNow,
                PrazoEntrega = DefinicoesTemporarias.DateTimeNow
            };
            MainPage.Current.Navegar<ManipulacaoProdutosRV>(rv);
        }

        void AbrirConfiguracoes(object sender, TappedRoutedEventArgs e) => Navegar<Configuracoes>();
        void AbrirImportacao(object sender, TappedRoutedEventArgs e) => Navegar<ImportacaoDados>();
        void AbrirInformacoes(object sender, TappedRoutedEventArgs e) => Navegar<Informacoes>();

        void AbrirCertificacao(object sender, TappedRoutedEventArgs e)
        {
            switch (ConfiguracoesCertificacao.Origem)
            {
                case OrigemCertificado.Importado:
                    MainPage.Current.Navegar<ConfiguracoesCertificadoImportado>();
                    break;
                case OrigemCertificado.Servidor:
                    MainPage.Current.Navegar<ConfiguracoesServidorCertificacao>();
                    break;
                case OrigemCertificado.Cliente:
                    MainPage.Current.Navegar<ConfiguracoesClienteServidor>();
                    break;
            }
        }

        void AbrirSincronizacao(object sender, TappedRoutedEventArgs e)
        {
            if (ConfiguracoesSincronizacao.Tipo == TipoAppSincronizacao.Cliente)
            {
                MainPage.Current.Navegar<SincronizacaoCliente>();
            }
            else
            {
                MainPage.Current.Navegar<SincronizacaoServidor>();
            }
        }

        void Navegar<T>(object param = null) where T : Page => MainPage.Current.Navegar<T>(param);

        async Task<bool> CriarNFeEntrada()
        {
            var caixa = new FileOpenPicker();
            caixa.FileTypeFilter.Add(".xml");
            var arq = await caixa.PickSingleFileAsync();
            if (arq != null)
            {
                try
                {
                    var xml = await ImportacaoDados.ObterXMLNFe(arq);
                    var proc = xml.FromXElement<ProcessoNFe>();
                    var nfe = proc.NFe;
                    if (nfe.Informacoes.destinatário.CNPJ == DefinicoesTemporarias.EmitenteAtivo.CNPJ)
                    {
                        using (var repo = new Repositorio.Leitura())
                        {
                            var c = repo.ObterClienteViaCNPJ(nfe.Informacoes.Emitente.CNPJ);
                            if (c != null)
                            {
                                nfe.Informacoes.destinatário = c.ToDestinatario();
                                nfe.Informacoes.Emitente = DefinicoesTemporarias.EmitenteAtivo.ToEmitente();
                                nfe.Informacoes.identificacao.TipoOperacao = 0;
                                var analisador = new AnalisadorNFe(ref nfe);
                                analisador.Desnormalizar();
                                var controle = new ControleNFe(nfe);
                                if (await new Criador(controle).ShowAsync() == ContentDialogResult.Primary)
                                {
                                    Popup.Current.Escrever(TitulosComuns.Sucesso, "Nota de entrada criada. Agora verifique se todas as informações estão corretas.");
                                    return true;
                                }
                            }
                            else
                            {
                                Popup.Current.Escrever(TitulosComuns.Atenção, "Para uma melhor esperiência na edição da NFe é preciso cadastrar o emitente da nota fiscal como cliente.\r\n" +
                                    "Após concluir o cadastro tente novamente criar a nota de entrada.");
                                var di = new ClienteDI(nfe.Informacoes.Emitente);
                                MainPage.Current.Navegar<AdicionarClienteBrasileiroPJ>(di);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        Popup.Current.Escrever(TitulosComuns.Atenção, "O cliente dessa nota fiscal não é o emitente ativo, por favor, escolha apenas notas fiscais para o emitente ativo.");
                    }
                }
                catch (Exception erro)
                {
                    erro.ManipularErro();
                }
            }
            return false;
        }
    }
}
