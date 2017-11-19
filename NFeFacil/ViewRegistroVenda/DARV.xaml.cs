using NFeFacil.ItensBD;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using System;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DARV : Page
    {
        GerenciadorImpressao gerenciador = new GerenciadorImpressao();
        ConjuntoDadosDARV Dados { get; set; }
        DadosImpressaoDARV DadosImpressao { get; set; }
        const int paddingPadrao = 1;

        public DARV()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DadosImpressao = (DadosImpressaoDARV)e.Parameter;
            var original = DadosImpressao.Venda;

            using (var db = new AplicativoContext())
            {
                var emitente = db.Emitentes.Find(original.Emitente);
                var vendedor = original.Vendedor != Guid.Empty ? db.Vendedores.Find(original.Vendedor) : null;
                var cliente = db.Clientes.Find(original.Cliente);
                var motorista = original.Motorista != Guid.Empty ? db.Motoristas.Find(original.Motorista) : null;

                var produtos = original.Produtos.Select(x => new DadosProduto
                {
                    Descricao = db.Produtos.Find(x.IdBase).Descricao,
                    Quantidade = x.Quantidade.ToString("N2"),
                    ValorUnitario = x.ValorUnitario.ToString("C2"),
                    Desconto = x.Desconto.ToString("C2"),
                    Adicionais = (x.DespesasExtras + x.Frete + x.Seguro).ToString("C2"),
                    Total = x.TotalLíquido.ToString("C2")
                });
                var array = original.Id.ToByteArray();
                var construtor = new StringBuilder();
                for (int i = 0; i < array.Length; i++)
                {
                    construtor.Append(array[i].ToString("000"));
                }
                var idSimplificado = construtor.ToString();
                construtor.Clear();
                var idOriginal = original.Id.ToString().ToUpper();

                Dados = new ConjuntoDadosDARV
                {
                    Emitente = new DadosEmitente
                    {
                        NomeFicticio = emitente.NomeFantasia,
                        Endereco = emitente.ToEmitente().Endereco
                    },
                    Cliente = new DadosCliente
                    {
                        Nome = cliente.Nome,
                        Endereco = cliente.ToDestinatario().Endereco
                    },
                    DataVenda = original.DataHoraVenda.ToString("dd-MM-yyyy"),
                    IdVenda = idOriginal,
                    ChaveNFeRelacionada = original.NotaFiscalRelacionada,
                    Vendedor = vendedor?.Nome ?? string.Empty,
                    CPFVendedor = vendedor?.CPF.ToString("000,000,000-00") ?? string.Empty,
                    Motorista = motorista?.Nome ?? string.Empty,
                    Produtos = produtos.ToArray(),
                    Desconto = original.DescontoTotal.ToString("C2"),
                    Adicionais = original.Produtos.Sum(x => x.DespesasExtras + x.Frete + x.Seguro).ToString("C2"),
                    Total = (original.Produtos.Sum(x => x.TotalLíquido) - original.DescontoTotal).ToString("C2"),
                    Observacoes = original.Observações
                };
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            gerenciador.Dispose();
            base.OnNavigatingFrom(e);
        }

        void PaginaPrincipalCarregada(object sender, RoutedEventArgs e)
        {
            var dimensoes = DadosImpressao.Dimensoes;
            DefinirTamanho(dimensoes);
            var grid = (Grid)sender;
            if (double.IsNaN(grid.Height))
            {
                listaPrincipal.ItemsSource = Dados.Produtos.GerarObs();
                linhaProdutos.Height = new GridLength(1, GridUnitType.Auto);
                grdPaginaPrincipal.VerticalAlignment = VerticalAlignment.Top;
            }
            else
            {
                var altura = linhaProdutos.ActualHeight;
                int nItens1Pag = (int)((altura / 22) - 1);
                if (Dados.Produtos.Length <= nItens1Pag)
                {
                    listaPrincipal.ItemsSource = Dados.Produtos.GerarObs();
                }
                else if (nItens1Pag <= 0)
                {
                    grdPaginaPrincipal.Children.Remove(listaPrincipal);
                    linhaProdutos.Height = new GridLength(1, GridUnitType.Auto);
                    int nItens2Pag = (int)(((dimensoes.AlturaProcessada - (2 * paddingPadrao)) / 22) - 1);
                    var nPaginas = Math.Ceiling((float)Dados.Produtos.Length / nItens2Pag);
                    for (int i = 0; i < nPaginas; i++)
                    {
                        var quantRestante = Dados.Produtos.Length - nItens2Pag * i;
                        var nItensAtual = quantRestante > nItens2Pag ? nItens2Pag : quantRestante;
                        var produtos = new DadosProduto[nItensAtual];
                        for (int k = 0; k < nItensAtual; k++)
                        {
                            produtos[k] = Dados.Produtos[nItens2Pag * i + k];
                        }
                        CriarPaginaFilho(produtos);
                    }
                    DefinirTamanho(dimensoes);
                }
                else
                {
                    listaPrincipal.ItemsSource = Dados.Produtos.Take(nItens1Pag).GerarObs();
                    int nItens2Pag = (int)(((dimensoes.AlturaProcessada - (2  * paddingPadrao)) / 22) - 1);
                    var nPaginas = Math.Ceiling((float)Dados.Produtos.Length / nItens2Pag);
                    for (int i = 0; i < nPaginas; i++)
                    {
                        var quantRestante = Dados.Produtos.Length - nItens1Pag - (nItens2Pag * i);
                        var nItensAtual = quantRestante > nItens2Pag ? nItens2Pag : quantRestante;
                        var produtos = new DadosProduto[nItensAtual];
                        for (int k = 0; k < nItensAtual; k++)
                        {
                            produtos[k] = Dados.Produtos[nItens1Pag + (nItens2Pag * i) + k];
                        }
                        CriarPaginaFilho(produtos);
                    }
                    DefinirTamanho(dimensoes);
                }
            }
        }

        void DefinirTamanho(Dimensoes dimensoes)
        {
            var filhos = paiPaginas.Children;
            for (int i = 0; i < filhos.Count; i++)
            {
                var filho = (Grid)filhos[i];
                filho.Width = dimensoes.LarguraProcessada;
                filho.Height = dimensoes.AlturaProcessada;
                filho.Padding = new Thickness(dimensoes.PaddingProcessado);
            }
        }

        void CriarPaginaFilho(DadosProduto[] produtos)
        {
            var lista = new ListView()
            {
                VerticalAlignment = VerticalAlignment.Top,
                Style = listaPadrao
            };
            lista.ItemsSource = produtos.GerarObs();

            var grid = new Grid();
            grid.Children.Add(lista);
            paiPaginas.Children.Add(grid);
        }

        private async void Imprimir(object sender, RoutedEventArgs e)
        {
            await gerenciador.Imprimir(paiPaginas.Children);
        }
    }

    public struct Dimensoes
    {
        public Dimensoes(double largura, double altura, double padding)
        {
            var novaLargura = CentimeterToPixel(largura);
            var novaAltura = altura != 0 ? CentimeterToPixel(altura) : double.NaN;
            var novoPadding = CentimeterToPixel(padding);

            LarguraOriginal = largura;
            LarguraProcessada = novaLargura;
            AlturaOriginal = altura;
            AlturaProcessada = novaAltura;
            PaddingOriginal = padding;
            PaddingProcessado = novoPadding;
        }

        public double LarguraOriginal { get; set; }
        public double AlturaOriginal { get; set; }
        public double PaddingOriginal { get; set; }

        public double LarguraProcessada { get; set; }
        public double AlturaProcessada { get; set; }
        public double PaddingProcessado { get; set; }
    }

    public struct ConjuntoDadosDARV
    {
        public DadosEmitente Emitente { get; set; }
        public DadosCliente Cliente { get; set; }
        public string DataVenda { get; set; }
        public string IdVenda { get; set; }
        public string ChaveNFeRelacionada { get; set; }
        public string Vendedor { get; set; }
        public string CPFVendedor { get; set; }
        public string Motorista { get; set; }
        public DadosProduto[] Produtos { get; set; }
        public string Desconto { get; set; }
        public string Adicionais { get; set; }
        public string Total { get; set; }
        public string Observacoes { get; set; }
    }

    public struct DadosEmitente
    {
        public string NomeFicticio { get; set; }
        public EnderecoCompleto Endereco { get; set; }
    }

    public struct DadosCliente
    {
        public string Nome { get; set; }
        public EnderecoCompleto Endereco { get; set; }
    }

    public struct DadosProduto
    {
        public string Descricao { get; set; }
        public string Quantidade { get; set; }
        public string ValorUnitario { get; set; }
        public string Desconto { get; set; }
        public string Adicionais { get; set; }
        public string Total { get; set; }
    }

    public struct DadosImpressaoDARV
    {
        public RegistroVenda Venda { get; set; }
        public Dimensoes Dimensoes { get; set; }
    }
}
