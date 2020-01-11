using System;
using BaseGeral.ItensBD;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using BaseGeral;
using BaseGeral.View;
using Venda;
using System.Threading.Tasks;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum.DARV
{
    [DetalhePagina(Symbol.View, "DARV")]
    public sealed partial class ViewDARV : Page
    {
        public double AlturaEscolhida { get; set; }
        public double LarguraEscolhida { get; set; }
        public Thickness PaddingEscolhido { get; set; }

        public RegistroVenda Registro { get; set; }
        public EmitenteDI Emitente { get; set; }
        public ClienteDI Cliente { get; set; }
        public string Vendedor { get; set; }
        public Comprador Comprador { get; set; }
        public MotoristaDI Motorista { get; set; }

        public string Subtotal { get; set; }
        public string Acrescimos { get; set; }
        public string Desconto { get; set; }
        public string Total { get; set; }

        public string Id => Registro.Id.ToString().ToUpper();
        public string NomeAssinatura => Comprador?.Nome ?? Cliente.Nome;
        public string Observacoes => Registro.Observações;

        public string EnderecoCliente { get; set; }
        public ExibicaoProduto[] ListaProdutos;

        public Visibility VisibilidadeTransporte { get; set; }
        public Visibility VisibilidadeNFeRelacionada { get; set; }
        public Visibility VisibilidadeComprador { get; set; }
        public Visibility VisibilidadePagamento { get; set; }
        public Visibility VisibilidadeObservacoes { get; set; }

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

            ListaProdutos = produtos.OrderBy(x => x.Descricao).ToArray();
            /*ListaProdutos = new ExibicaoProduto[100];
            for (int i = 0; i < 100; i++)
            {
                ListaProdutos[i] = produtos[0];
                ListaProdutos[i].CodigoProduto = i.ToString("000");
            }*/

            if (!string.IsNullOrEmpty(Cliente.CPF))
                EnderecoCliente = ObterEnderecoClienteFisico(Cliente);
            else if (!string.IsNullOrEmpty(Cliente.CNPJ))
                EnderecoCliente = ObterEnderecoClienteJuridico(Cliente);
            else
                EnderecoCliente = ObterEnderecoClienteExterior(Cliente);

            VisibilidadeTransporte = ToVis(Motorista == null);
            VisibilidadeNFeRelacionada = ToVis(string.IsNullOrEmpty(Registro.NotaFiscalRelacionada));
            VisibilidadeComprador = ToVis(Comprador == null);
            VisibilidadePagamento = ToVis(string.IsNullOrEmpty(Registro.FormaPagamento) && string.IsNullOrEmpty(Registro.CondicaoPagamento));
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

        async void Pagina0Carregada(object sender, RoutedEventArgs e)
        {
            var alturaDisponivel = alturaLinhaProdutos.ActualHeight - 20;
            int quantMaxima = (int)Math.Floor(alturaDisponivel / 21) - 1;
            if (quantMaxima >= ListaProdutos.Length)
            {
                produtosPagina0.Content = new ProdutosDARV() { Produtos = ListaProdutos.GerarObs() };
                alturaLinhaProdutos.Height = new GridLength(1, GridUnitType.Auto);
                alturaFinalProdutos.Height = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                rodMainPag.Visibility = Visibility.Collapsed;
                await Task.Delay(500);

                var quantMaximaPaginaExtra = (int)Math.Floor(alturaLinhaProdutos.ActualHeight / 21);
                produtosPagina0.Content = new ProdutosDARV() { Produtos = ListaProdutos.Take(quantMaximaPaginaExtra).GerarObs() };
                var quantProdutosAdicionados = quantMaximaPaginaExtra;
                for (int i = 1; quantProdutosAdicionados < ListaProdutos.Length; i++, quantProdutosAdicionados += quantMaximaPaginaExtra)
                {
                    var quantProdutosIgnorados = quantMaximaPaginaExtra * i;
                    bool isUltima = quantProdutosAdicionados >= ListaProdutos.Length;
                    if (!isUltima) isUltima = quantProdutosAdicionados + quantMaxima >= ListaProdutos.Length;
                    ConteinerPaginas.Children.Add(
                        new PaginaAdicional(
                            new ProdutosDARV
                            {
                                Produtos = ListaProdutos
                                    .Skip(quantProdutosIgnorados)
                                    .Take(isUltima ? quantMaxima : quantMaximaPaginaExtra)
                                    .GerarObs()
                            })
                        {
                            Padding = PaddingEscolhido,
                            Height = AlturaEscolhida,
                            Width = LarguraEscolhida,
                            Main = this,
                            IsUltimaPagina = isUltima ? Visibility.Visible : Visibility.Collapsed
                        });
                }
            }
        }

        async void Imprimir(object sender, RoutedEventArgs e)
        {
            await Gerenciador.Imprimir(ConteinerPaginas.Children);
        }
    }
}
