using System;
using NFeFacil.ItensBD;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static NFeFacil.ExtensoesPrincipal;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using System.Linq;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DARV2 : Page
    {
        internal GridLength Largura2 { get; } = CentimeterToLength(2);
        internal GridLength Largura3 { get; } = CentimeterToLength(3);

        internal double AlturaEscolhida { get; private set; }
        internal double LarguraEscolhida { get; private set; }
        internal double PaddingEscolhido { get; private set; }

        internal RegistroVenda Registro { get; private set; }
        internal EmitenteDI Emitente { get; private set; }
        internal ClienteDI Cliente { get; private set; }
        internal Vendedor Vendedor { get; private set; }
        internal Comprador Comprador { get; private set; }
        internal MotoristaDI Motorista { get; private set; }

        internal string Subtotal { get; private set; }
        internal string Acrescimos { get; private set; }
        internal string Desconto { get; private set; }
        internal string Total { get; private set; }

        internal string NomeAssinatura => Comprador?.Nome ?? Cliente.Nome;
        internal string Observacoes => Registro.Observações;

        internal DataTemplate TemplateCliente { get; private set; }
        internal DataTemplate TemplateTransporte { get; private set; }

        ExibicaoProduto[] ListaProdutos;

        public DARV2()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            #region Processamento de dados

            DadosImpressaoDARV venda = (DadosImpressaoDARV)e.Parameter;
            var dimensoes = venda.Dimensoes;
            var registro = venda.Venda;

            AlturaEscolhida = dimensoes.AlturaProcessada;
            LarguraEscolhida = dimensoes.LarguraProcessada;
            PaddingEscolhido = dimensoes.PaddingProcessado;

            Registro = registro;
            Emitente = Propriedades.EmitenteAtivo;
            ExibicaoProduto[] produtos = new ExibicaoProduto[registro.Produtos.Count];
            double subtotal = 0;
            double acrescimos = 0;
            double desconto = 0;
            using (var db = new AplicativoContext())
            {
                Cliente = db.Clientes.Find(registro.Cliente);
                Vendedor = db.Vendedores.Find(registro.Vendedor);
                Comprador = db.Compradores.Find(registro.Comprador);
                Motorista = db.Motoristas.Find(registro.Motorista);

                for (var i = 0; i < registro.Produtos.Count; i++)
                {
                    var atual = registro.Produtos[i];
                    var completo = db.Produtos.Find(atual.IdBase);
                    var totBruto = atual.Quantidade * atual.ValorUnitario;
                    subtotal += totBruto;
                    acrescimos += atual.DespesasExtras + atual.Frete + atual.Seguro;
                    desconto += atual.Desconto;
                    produtos[i] = new ExibicaoProduto
                    {
                        CodigoProduto = completo.CodigoProduto,
                        Descricao = completo.Descricao,
                        ValorUnitario = atual.ValorUnitario.ToString("C2"),
                        TotalBruto = totBruto.ToString("C2")
                    };
                }
            }

            Subtotal = subtotal.ToString("C2");
            Acrescimos = acrescimos.ToString("C2");
            Desconto = desconto.ToString("C2");
            Total = (subtotal + acrescimos + desconto).ToString("C2");

            #endregion

            #region Analise visual

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

            #endregion
        }

        private void Pagina0Carregada(object sender, RoutedEventArgs e)
        {
            var alturaDisponivel = alturaLinhaProdutos.ActualHeight - 20;
            int quantMaxima = (int)Math.Floor(alturaDisponivel / 20);
            if (quantMaxima <= ListaProdutos.Length)
            {
                produtosPagina0.DataContext = ListaProdutos.GerarObs();
            }
            else
            {
                produtosPagina0.DataContext = ListaProdutos.Take(quantMaxima).GerarObs();

                var espacoDisponivelPaginaExtra = AlturaEscolhida - (PaddingEscolhido * 2);
                var quantMaximaPaginaExtra = Math.Floor(espacoDisponivelPaginaExtra / 20);
                var quantProdutosRestantes = ListaProdutos.Length - quantMaxima;
                int quantPaginasExtras = (int)Math.Ceiling(quantProdutosRestantes / quantMaximaPaginaExtra);
                for (int i = 0; i < quantPaginasExtras; i++)
                {
                    var quantProdutosIgnorados = quantMaxima + ((int)quantMaximaPaginaExtra * i);
                    ConteinerPaginas.Children.Add(new ContentPresenter
                    {
                        ContentTemplate = Produtos,
                        Padding = new Thickness(PaddingEscolhido),
                        Height = AlturaEscolhida,
                        Width = LarguraEscolhida,
                        DataContext = ListaProdutos.Skip(quantProdutosIgnorados).Take((int)quantMaximaPaginaExtra)
                    });
                }
            }
        }

        struct ExibicaoProduto
        {
            public string CodigoProduto { get; set; }
            public string Descricao { get; set; }
            public string ValorUnitario { get; set; }
            public string TotalBruto { get; set; }
        }
    }

    sealed class MascaraDocumento : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            long numero;
            if (value is string str) numero = long.Parse(str.Trim());
            else if (value is long num) numero = num;
            else throw new InvalidCastException();

            if (numero >= (10 ^ 13))
                return numero.ToString("00.000.000/0000-00");
            else if (numero >= (10 ^ 10))
                return numero.ToString("000.000.000-00");
            else
                return numero.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
