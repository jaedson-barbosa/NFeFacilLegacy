using System;
using NFeFacil.ItensBD;
using Windows.UI.Xaml.Controls;
using static NFeFacil.ExtensoesPrincipal;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using System.Collections.ObjectModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DARV : Page
    {
        internal double AlturaEscolhida { get; private set; }
        internal double LarguraEscolhida { get; private set; }
        internal Thickness PaddingEscolhido { get; private set; }

        internal RegistroVenda Registro { get; private set; }
        internal EmitenteDI Emitente { get; private set; }
        internal ClienteDI Cliente { get; private set; }
        internal string Vendedor { get; private set; }
        internal Comprador Comprador { get; private set; }
        internal MotoristaDI Motorista { get; private set; }

        internal string Subtotal { get; private set; }
        internal string Acrescimos { get; private set; }
        internal string Desconto { get; private set; }
        internal string Total { get; private set; }

        string Id => Registro.Id.ToString().ToUpper();
        internal string NomeAssinatura => Comprador?.Nome ?? Cliente.Nome;
        internal string Observacoes => Registro.Observações;

        internal DataTemplate TemplateCliente { get; private set; }
        internal DataTemplate TemplateTransporte { get; private set; }

        ExibicaoProduto[] ListaProdutos;

        internal Visibility VisibilidadeNFeRelacionada { get; private set; }
        internal Visibility VisibilidadeComprador { get; private set; }
        internal Visibility VisibilidadePagamento { get; private set; }
        internal Visibility VisibilidadeObservacoes { get; private set; }

        readonly GerenciadorImpressao Gerenciador = new GerenciadorImpressao();
        DARV This { get; }

        public DARV()
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
            Emitente = Propriedades.EmitenteAtivo;
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
            Total = (subtotal + acrescimos + desconto).ToString("C2");

            ListaProdutos = produtos;

            if (!string.IsNullOrEmpty(Cliente.CPF))
                TemplateCliente = ClienteFisico;
            else if (!string.IsNullOrEmpty(Cliente.CNPJ))
                TemplateCliente = ClienteJuridico;
            else
                TemplateCliente = ClienteExterior;

            if (!string.IsNullOrEmpty(Motorista.CPF))
                TemplateTransporte = TransporteFisico;
            else if (!string.IsNullOrEmpty(Motorista.CNPJ))
                TemplateTransporte = TransporteJuridico;

            VisibilidadeNFeRelacionada = string.IsNullOrEmpty(Registro.NotaFiscalRelacionada) ? Visibility.Collapsed : Visibility.Visible;
            VisibilidadeComprador = Comprador == null ? Visibility.Collapsed : Visibility.Visible;
            VisibilidadePagamento = string.IsNullOrEmpty(Registro.FormaPagamento) ? Visibility.Collapsed : Visibility.Visible;
            VisibilidadeObservacoes = string.IsNullOrEmpty(Registro.Observações) ? Visibility.Collapsed : Visibility.Visible;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Gerenciador.Dispose();
        }

        private void Pagina0Carregada(object sender, RoutedEventArgs e)
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
            alturaLinhaFinalProdutos.Height = new GridLength(1, GridUnitType.Star);
        }

        async void Imprimir(object sender, RoutedEventArgs e)
        {
            await Gerenciador.Imprimir(ConteinerPaginas.Children);
        }
    }

    public struct ExibicaoProduto
    {
        public string Quantidade { get; set; }
        public string CodigoProduto { get; set; }
        public string Descricao { get; set; }
        public string ValorUnitario { get; set; }
        public string TotalBruto { get; set; }
    }

    public sealed class ProdutosDARV
    {
        public ObservableCollection<ExibicaoProduto> Produtos { get; set; }

        public static GridLength Largura2 { get; } = CMToLength(2);
        public static GridLength Largura3 { get; } = CMToLength(3);
    }

    public struct Dimensoes
    {
        public Dimensoes(double largura, double altura, double padding)
        {
            LarguraOriginal = largura;
            LarguraProcessada = CMToPixel(largura);
            AlturaOriginal = altura;
            AlturaProcessada = altura != 0 ? CMToPixel(altura) : double.NaN;
            PaddingOriginal = padding;
            PaddingProcessado = CMToPixel(padding);
        }

        public double LarguraOriginal { get; set; }
        public double AlturaOriginal { get; set; }
        public double PaddingOriginal { get; set; }

        public double LarguraProcessada { get; set; }
        public double AlturaProcessada { get; set; }
        public double PaddingProcessado { get; set; }
    }

    public struct DadosImpressaoDARV
    {
        public RegistroVenda Venda { get; set; }
        public Dimensoes Dimensoes { get; set; }
        public ClienteDI Cliente { get; set; }
        public MotoristaDI Motorista { get; set; }
        public Vendedor Vendedor { get; set; }
        public Comprador Comprador { get; set; }
        public ProdutoDI[] ProdutosCompletos { get; set; }
    }
}
