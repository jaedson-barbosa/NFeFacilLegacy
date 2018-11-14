using Fiscal.Certificacao;
using BaseGeral.ItensBD;
using BaseGeral.Log;
using BaseGeral.ModeloXML;
using Venda.GerenciamentoProdutos;
using NFeFacil.Sincronizacao;
using BaseGeral.Validacao;
using NFeFacil.ViewDadosBase;
using Comum;
using RegistroComum;
using System;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Fiscal;
using Windows.UI.Xaml.Navigation;
using BaseGeral;
using BaseGeral.Sincronizacao;
using BaseGeral.View;
using Consumidor;
using Windows.UI.Xaml;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    [DetalhePagina(Symbol.Home, "Início")]
    public sealed partial class Inicio : Page
    {
        bool produtosEFornecedoresCadastrados;

        public Inicio()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var comprado = ComprasInApp.Resumo[Compras.NFCe];
            hubNFCe.Visibility = comprado ? Visibility.Visible : Visibility.Collapsed;
            using (var leitura = new BaseGeral.Repositorio.Leitura())
                produtosEFornecedoresCadastrados = leitura.ExisteCategoria && leitura.ExisteFornecedor;
        }

        void AbrirClientes(object sender, RoutedEventArgs e) => Navegar<GerenciarClientes>();
        void AbrirMotoristas(object sender, RoutedEventArgs e) => Navegar<GerenciarMotoristas>();
        void AbrirProdutos(object sender, RoutedEventArgs e) => Navegar<GerenciarProdutos>();
        void AbrirFornecedores(object sender, RoutedEventArgs e) => Navegar<GerenciarFornecedores>();
        void AbrirCategorias(object sender, RoutedEventArgs e) => Navegar<GerenciarCategorias>();
        void AbrirVendedores(object sender, RoutedEventArgs e) => Navegar<GerenciarVendedores>();
        void AbrirCompradores(object sender, RoutedEventArgs e) => Navegar<GerenciarCompradores>();

        async void CriarNFe(object sender, RoutedEventArgs e)
        {
            var controle = new ControleNFe();
            await new Criador(controle).ShowAsync();
        }

        void CriarNFeEntrada(object sender, RoutedEventArgs e) => CriarNFeEntrada();
        void AbrirInutilizacoes(object sender, RoutedEventArgs e) => Navegar<Inutilizacoes>();
        void AbrirNotasSalvas(object sender, RoutedEventArgs e) => Navegar<NotasSalvas>(new ControleViewNFe());
        void AbrirConsulta(object sender, RoutedEventArgs e) => Navegar<Consulta>();
        void AbrirVendasAnuais(object sender, RoutedEventArgs e) => Navegar<VendasAnuais>();

        async void CriarNFCe(object sender, RoutedEventArgs e)
        {
            var controle = new ControleNFCe();
            await new Criador(controle).ShowAsync();
        }
        void AbrirInutilizacoesNFCe(object sender, RoutedEventArgs e) => Navegar<Inutilizacoes>(true);
        void AbrirNFCesSalvas(object sender, RoutedEventArgs e) => Navegar<NotasSalvas>(new ControleViewNFCe());
        void AbrirConsultaNFCe(object sender, RoutedEventArgs e) => Navegar<Consulta>(true);
        void AbrirVendasAnuaisNFCe(object sender, RoutedEventArgs e) => Navegar<VendasAnuais>(true);

        void AbrirVendasSalvas(object sender, RoutedEventArgs e) => Navegar<RegistrosVenda>();
        void AbrirRelatorioProdutos01(object sender, RoutedEventArgs e)
        {
            if (!ComprasInApp.Resumo[Compras.RelatorioProdutos01])
                Popup.Current.Escrever(TitulosComuns.Atenção, "Primeiro você precisa comprar este adicional na aba de compras da tela de configurações.");
            else if (produtosEFornecedoresCadastrados)
                Navegar<RegistroComum.RelatorioProduto01.GeradorRelatorioProduto01>();
            else
                Popup.Current.Escrever(TitulosComuns.Atenção, "Primeiro você precisa cadastrar categorias e fornecedores para os seus produtos.");
        }

        void CriarVenda(object sender, RoutedEventArgs e)
        {
            var controle = new RegistroComum.ControleViewProduto();
            Navegar<Venda.ViewProdutoVenda.ListaProdutos>(controle);
        }

        void AbrirConfiguracoes(object sender, RoutedEventArgs e) => Navegar<Configuracoes>();
        void AbrirImportacao(object sender, RoutedEventArgs e) => Navegar<ImportacaoDados>();
        void AbrirInformacoes(object sender, RoutedEventArgs e) => Navegar<Informacoes>();

        void AbrirCertificacao(object sender, RoutedEventArgs e)
        {
            switch (ConfiguracoesCertificacao.Origem)
            {
                case OrigemCertificado.Importado:
                    MainPage.Current.Navegar<ConfiguracoesCertificadoImportado>();
                    break;
                case OrigemCertificado.Servidor:
                    MainPage.Current.Navegar<ConfiguracoesServidorCertificacao>();
                    break;
            }
        }

        void AbrirSincronizacao(object sender, RoutedEventArgs e)
        {
            if (ConfiguracoesSincronizacao.Tipo == TipoAppSincronizacao.Cliente)
                MainPage.Current.Navegar<SincronizacaoCliente>();
            else
                MainPage.Current.Navegar<SincronizacaoServidor>();
        }

        void Navegar<T>(object param = null) where T : Page => MainPage.Current.Navegar<T>(param);

        async void CriarNFeEntrada()
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
                    if (nfe.Informacoes.destinatario.CNPJ == DefinicoesTemporarias.EmitenteAtivo.CNPJ)
                    {
                        ClienteDI c;
                        using (var repo = new BaseGeral.Repositorio.Leitura())
                        {
                            c = repo.ObterClienteViaCNPJ(nfe.Informacoes.Emitente.CNPJ);
                        }
                        if (c != null)
                        {
                            nfe.Informacoes.destinatario = c.ToDestinatario();
                            nfe.Informacoes.Emitente = DefinicoesTemporarias.EmitenteAtivo.ToEmitente();
                            nfe.Informacoes.identificacao.TipoOperacao = 0;
                            var analisador = new AnalisadorNFe(ref nfe);
                            analisador.Desnormalizar();
                            var controle = new ControleNFe(nfe);
                            if (await new Criador(controle).ShowAsync() == ContentDialogResult.Primary)
                            {
                                Popup.Current.Escrever(TitulosComuns.Sucesso, "Nota de entrada criada. Agora verifique se todas as informações estão corretas.");
                            }
                        }
                        else
                        {
                            using (var repo = new BaseGeral.Repositorio.Escrita())
                            {
                                repo.SalvarItemSimples(new ClienteDI(nfe.Informacoes.Emitente),
                                    DefinicoesTemporarias.DateTimeNow);
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
        }
    }
}
