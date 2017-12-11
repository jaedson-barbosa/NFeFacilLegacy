using NFeFacil.Certificacao;
using NFeFacil.DANFE;
using NFeFacil.Importacao;
using NFeFacil.Login;
using NFeFacil.Sincronizacao;
using NFeFacil.View;
using NFeFacil.ViewDadosBase;
using NFeFacil.ViewDadosBase.GerenciamentoProdutos;
using NFeFacil.ViewNFe;
using NFeFacil.ViewNFe.CaixasEspeciaisProduto;
using NFeFacil.ViewRegistroVenda;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NFeFacil
{
    public sealed class PaginasPrincipais : Attribute
    {
        public static readonly Dictionary<Type, Pagina> Lista = new Dictionary<Type, Pagina>
        {
            { typeof(ConfiguracoesCertificadoImportado), (Symbol.Permissions, "Certificação") },
            { typeof(ConfiguracoesClienteServidor), (Symbol.Permissions, "Certificação") },
            { typeof(ConfiguracoesServidorCertificacao), (Symbol.Permissions, "Certificação") },
            { typeof(ViewDANFE), (Symbol.View, "DANFE") },
            { typeof(ImportacaoDados), (Symbol.Import, "Importação") },
            { typeof(AdicionarEmitente), (Symbol.People, "Emitente") },
            { typeof(EscolhaEmitente), (Symbol.Home, "Escolher empresa") },
            { typeof(EscolhaVendedor), (Symbol.Home, "Escolher vendedor") },
            { typeof(GeralEmitente), (Symbol.Home, "Dados da empresa") },
            { typeof(PrimeiroUso), (Symbol.Emoji, "Bem-vindo") },
            { typeof(QRConexao), (Symbol.View, "QR") },
            { typeof(SincronizacaoCliente), ("\uE975", "Sincronização") },
            { typeof(SincronizacaoServidor), ("\uE977", "Sincronização") },
            { typeof(AdicionarComprador), (Symbol.People, "Comprador") },
            { typeof(AdicionarMotorista), ("\uE806", "Motorista") },
            { typeof(ViewDadosBase.GerenciamentoProdutos.AdicionarProduto), (Symbol.Shop, "Produto") },
            { typeof(AdicionarVendedor), (Symbol.People, "Vendedor") },
            { typeof(GerenciarDadosBase), (Symbol.Manage, "Dados base") },
            { typeof(DefinirArmamentos), (new Uri(ObterCaminhoArmamento()), "Armamento") },
            { typeof(DefinirCombustivel), ("\uEB42", "Combustivel") },
            { typeof(DefinirMedicamentos), ("\uE95E", "Medicamentos") },
            { typeof(DefinirVeiculo), ("\uE804", "Veículo") },
            { typeof(ImpostosProduto), ("\uE825", "Impostos") },
            { typeof(ManipulacaoNotaFiscal), (Symbol.Document, "Nota fiscal") },
            { typeof(ManipulacaoProdutoCompleto), (Symbol.Shop, "Produto") },
            { typeof(NotasSalvas), (Symbol.Library, "Notas salvas") },
            { typeof(VisualizacaoNFe), (Symbol.View, "Visualizar NFe") },
            { typeof(ManipulacaoRegistroVenda), ("\uEC59", "Registro de venda") },
            { typeof(RegistrosVenda), (Symbol.Library, "Vendas") },
            { typeof(VisualizacaoRegistroVenda), (Symbol.View, "Registro de venda") },
            { typeof(ControleEstoque), (Symbol.Manage, "Controle de estoque") },
            { typeof(Inicio), (Symbol.Home, "Início") },
            { typeof(VendasAnuais), (Symbol.Calendar, "Vendas") },
            { typeof(Backup), ("\uEA35", "Backup") },
            { typeof(Consulta), (Symbol.Find, "Consulta") },
            { typeof(AdicionarClienteBrasileiroPFContribuinte), (Symbol.People, "Cliente") },
            { typeof(AdicionarClienteEstrangeiro), (Symbol.People, "Cliente") },
            { typeof(AdicionarClienteBrasileiroPF), (Symbol.People, "Cliente") },
            { typeof(AdicionarClienteBrasileiroPJ), (Symbol.People, "Cliente") },
            { typeof(Configuracoes), (Symbol.Setting, "Configurações") },
            { typeof(GerenciarClientes), (Symbol.Manage, "Clientes") },
            { typeof(GerenciarCompradores), (Symbol.Manage, "Compradores") },
            { typeof(GerenciarMotoristas), (Symbol.Manage, "Gerenciar motoristas") },
            { typeof(GerenciarProdutos), (Symbol.Manage, "Gerenciar produtos") },
            { typeof(GerenciarVendedores), (Symbol.Manage, "Gerenciar vendedores") },
            { typeof(Informacoes), ("\uE946", "Informações") }
        };

        static string ObterCaminhoArmamento()
        {
            var usarDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
            return usarDark ? "ms-appx:///Assets/ArmaDark.png" : "ms-appx:///Assets/Arma.png";
        }

        public struct Pagina
        {
            public string Titulo { get; }
            Uri SimboloUri { get; }
            string SimboloGlyph { get; }
            Symbol SimboloSymbol { get; }

            public Pagina(Uri caminho, string texto)
            {
                SimboloUri = caminho;
                SimboloGlyph = null;
                SimboloSymbol = default(Symbol);
                Titulo = texto;
            }

            public Pagina(string glyph, string texto)
            {
                SimboloUri = null;
                SimboloGlyph = glyph;
                SimboloSymbol = default(Symbol);
                Titulo = texto;
            }

            public Pagina(Symbol símbolo, string texto)
            {
                SimboloUri = null;
                SimboloGlyph = null;
                SimboloSymbol = símbolo;
                Titulo = texto;
            }

            public IconElement ObterIcone()
            {
                if (SimboloUri != null)
                {
                    return new BitmapIcon { UriSource = SimboloUri };
                }
                else if (SimboloGlyph != null)
                {
                    return new FontIcon { Glyph = SimboloGlyph };
                }
                else
                {
                    return new SymbolIcon(SimboloSymbol);
                }
            }

            public static implicit operator Pagina((Uri, string) x) => new Pagina(x.Item1, x.Item2);
            public static implicit operator Pagina((string, string) x) => new Pagina(x.Item1, x.Item2);
            public static implicit operator Pagina((Symbol, string) x) => new Pagina(x.Item1, x.Item2);
        }
    }
}
