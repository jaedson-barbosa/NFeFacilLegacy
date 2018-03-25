using System;
using BaseGeral.ItensBD;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using BaseGeral;
using BaseGeral.View;
using NFeFacil;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum.DARV
{
    [DetalhePagina(Symbol.View, "DARV")]
    public sealed partial class ViewDARV : Page
    {
        double AlturaEscolhida { get; set; }
        double LarguraEscolhida { get; set; }
        Thickness PaddingEscolhido { get; set; }

        RegistroVenda Registro { get; set; }
        EmitenteDI Emitente { get; set; }
        ClienteDI Cliente { get; set; }
        string Vendedor { get; set; }
        Comprador Comprador { get; set; }
        MotoristaDI Motorista { get; set; }

        string Subtotal { get; set; }
        string Acrescimos { get; set; }
        string Desconto { get; set; }
        string Total { get; set; }

        string Id => Registro.Id.ToString().ToUpper();
        string NomeAssinatura => Comprador?.Nome ?? Cliente.Nome;
        string Observacoes => Registro.Observações;

        string EnderecoCliente { get; set; }
        ExibicaoProduto[] ListaProdutos;

        Visibility VisibilidadeTransporte { get; set; }
        Visibility VisibilidadeNFeRelacionada { get; set; }
        Visibility VisibilidadeComprador { get; set; }
        Visibility VisibilidadePagamento { get; set; }
        Visibility VisibilidadeObservacoes { get; set; }

        readonly GerenciadorImpressao Gerenciador = new GerenciadorImpressao();
        ViewDARV This { get; }

        public ViewDARV()
        {
            InitializeComponent();
            This = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DadosImpressaoDARV venda = (DadosImpressaoDARV)e.Parameter;
            var dimensoes = venda.Dimensoes;
            var registro = venda.Venda;

            AlturaEscolhida = dimensoes.AlturaProcessada;
            LarguraEscolhida = dimensoes.LarguraProcessada;
            PaddingEscolhido = new Thickness(dimensoes.PaddingProcessado);

            Registro = registro;
            Emitente = DefinicoesTemporarias.EmitenteAtivo;
            Cliente = venda.Cliente;
            Vendedor = venda.Vendedor?.Nome ?? "Dono da empresa";
            Comprador = venda.Comprador;
            Motorista = venda.Motorista;

            ExibicaoProduto[] produtos = new ExibicaoProduto[registro.Produtos.Count];
            double subtotal = 0;
            double acrescimos = 0;
            double desconto = 0;
            for (var i = 0; i < registro.Produtos.Count; i++)
            {
                var atual = registro.Produtos[i];
                var completo = venda.ProdutosCompletos.First(x => x.Id == atual.IdBase);
                var totBruto = atual.Quantidade * atual.ValorUnitario;
                subtotal += totBruto;
                acrescimos += atual.DespesasExtras + atual.Frete + atual.Seguro;
                desconto += atual.Desconto;
                produtos[i] = new ExibicaoProduto
                {
                    Quantidade = atual.Quantidade.ToString("N2"),
                    CodigoProduto = completo.CodigoProduto,
                    Descricao = completo.Descricao,
                    ValorUnitario = atual.ValorUnitario.ToString("C2"),
                    TotalBruto = totBruto.ToString("C2")
                };
            }

            Subtotal = subtotal.ToString("C2");
            Acrescimos = acrescimos.ToString("C2");
            Desconto = desconto.ToString("C2");
            Total = (subtotal + acrescimos - desconto).ToString("C2");

            ListaProdutos = produtos;

            if (!string.IsNullOrEmpty(Cliente.CPF))
                EnderecoCliente = ObterEnderecoClienteFisico(Cliente);
            else if (!string.IsNullOrEmpty(Cliente.CNPJ))
                EnderecoCliente = ObterEnderecoClienteJuridico(Cliente);
            else
                EnderecoCliente = ObterEnderecoClienteExterior(Cliente);

            VisibilidadeTransporte = ToVis(Motorista == null);
            VisibilidadeNFeRelacionada = ToVis(string.IsNullOrEmpty(Registro.NotaFiscalRelacionada));
            VisibilidadeComprador = ToVis(Comprador == null);
            VisibilidadePagamento = ToVis(string.IsNullOrEmpty(Registro.FormaPagamento));
            VisibilidadeObservacoes = ToVis(string.IsNullOrEmpty(Registro.Observações));

            Visibility ToVis(bool esconde) => esconde ? Visibility.Collapsed : Visibility.Visible;
        }

        static string ObterEnderecoClienteFisico(ClienteDI cliente)
        {
            var cep = ExtensoesPrincipal.AplicarMáscaraDocumento(cliente.CEP);
            return $"{cliente.Logradouro}, {cliente.Numero} - {cliente.Bairro} - {cliente.NomeMunicipio}/{cliente.SiglaUF} (CEP: {cep})";
        }

        static string ObterEnderecoClienteJuridico(ClienteDI cliente)
        {
            var cep = ExtensoesPrincipal.AplicarMáscaraDocumento(cliente.CEP);
            return $"{cliente.Logradouro}, {cliente.Numero} - {cliente.Bairro} - {cliente.NomeMunicipio}/{cliente.SiglaUF} (CEP: {cep})";
        }

        static string ObterEnderecoClienteExterior(ClienteDI cliente)
        {
            var cep = ExtensoesPrincipal.AplicarMáscaraDocumento(cliente.CEP);
            return $"{cliente.Logradouro}, {cliente.Numero} - {cliente.Bairro} - {cliente.XPais}";
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Gerenciador.Dispose();
        }

        void Pagina0Carregada(object sender, RoutedEventArgs e)
        {
            var alturaDisponivel = alturaLinhaProdutos.ActualHeight - 20;
            int quantMaxima = (int)Math.Floor(alturaDisponivel / 20) - 1;
            if (quantMaxima <= ListaProdutos.Length)
            {
                produtosPagina0.Content = new ProdutosDARV() { Produtos = ListaProdutos.GerarObs() };
            }
            else
            {
                produtosPagina0.Content = new ProdutosDARV() { Produtos = ListaProdutos.Take(quantMaxima).GerarObs() };

                var espacoDisponivelPaginaExtra = AlturaEscolhida - (PaddingEscolhido.Bottom * 2);
                var quantMaximaPaginaExtra = Math.Floor(espacoDisponivelPaginaExtra / 20);
                var quantProdutosRestantes = ListaProdutos.Length - quantMaxima;
                int quantPaginasExtras = (int)Math.Ceiling(quantProdutosRestantes / quantMaximaPaginaExtra);
                for (int i = 0; i < quantPaginasExtras; i++)
                {
                    var quantProdutosIgnorados = quantMaxima + ((int)quantMaximaPaginaExtra * i);
                    ConteinerPaginas.Children.Add(new ContentPresenter
                    {
                        ContentTemplate = Produtos,
                        Padding = PaddingEscolhido,
                        Height = AlturaEscolhida,
                        Width = LarguraEscolhida,
                        Content = new ProdutosDARV
                        {
                            Produtos = ListaProdutos.Skip(quantProdutosIgnorados).Take((int)quantMaximaPaginaExtra).GerarObs()
                        }
                    });
                }
            }

            alturaLinhaProdutos.Height = new GridLength(1, GridUnitType.Auto);
            alturaFinalProdutos.Height = new GridLength(1, GridUnitType.Star);
        }

        async void Imprimir(object sender, RoutedEventArgs e)
        {
            await Gerenciador.Imprimir(ConteinerPaginas.Children);
        }
    }
}
