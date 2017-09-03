using NFeFacil.ItensBD;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
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
        const int paddingPadrao = 1;

        public DARV()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var original = (RegistroVenda)e.Parameter;

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
                    Valor = x.ValorUnitario.ToString("C2"),
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
                    Barras = idSimplificado,
                    ChaveNFeRelacionada = original.NotaFiscalRelacionada,
                    Vendedor = vendedor?.Nome ?? string.Empty,
                    CPFVendedor = vendedor?.CPF.ToString("000,000,000-00") ?? string.Empty,
                    Motorista = motorista?.Nome ?? string.Empty,
                    Produtos = produtos.ToArray(),
                    Desconto = original.DescontoTotal.ToString("C2"),
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

        private async void PaginaPrincipalCarregada(object sender, RoutedEventArgs e)
        {
            var dimensoes = await DefinirTamanho();
            if (dimensoes.AlturaOriginal < 15)
            {
                var caixa = new MessageDialog("A altura da página está muito pequena, você deseja omitir o código de barras?");
                caixa.Commands.Add(new UICommand("Sim", x => grdPaginaPrincipal.Children.Remove(codeBarras)));
                caixa.Commands.Add(new UICommand("Não"));
                await caixa.ShowAsync();
            }
            await Task.Delay(500);
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
                    DefinirTamanho(dimensoes.LarguraOriginal, dimensoes.AlturaOriginal, dimensoes.PaddingOriginal);
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
                    DefinirTamanho(dimensoes.LarguraOriginal, dimensoes.AlturaOriginal, dimensoes.PaddingOriginal);
                }
            }
        }

        async Task<Dimensoes> DefinirTamanho()
        {
            double largura = 21, altura = 29.7, padding = 1;
            var caixa = new EscolherDimensão();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                largura = caixa.Largura;
                if (caixa.FormularioContinuo)
                {
                    altura = 0;
                }
                else
                {
                    altura = caixa.Altura;
                }
            }
            return DefinirTamanho(largura, altura, padding);
        }

        Dimensoes DefinirTamanho(double largura, double altura, double padding)
        {
            var filhos = paiPaginas.Children;
            var novaLargura = CentimeterToPixel(largura);
            var novaAltura = altura != 0 ? CentimeterToPixel(altura) : double.NaN;
            var novoPadding = CentimeterToPixel(padding);
            for (int i = 0; i < filhos.Count; i++)
            {
                var filho = (Grid)filhos[i];
                filho.Width = novaLargura;
                filho.Height = novaAltura;
                filho.Padding = new Thickness(novoPadding);
            }
            return new Dimensoes
            {
                LarguraOriginal = largura,
                LarguraProcessada = novaLargura,
                AlturaOriginal = altura,
                AlturaProcessada = novaAltura,
                PaddingOriginal = padding,
                PaddingProcessado = novoPadding
            };
        }

        struct Dimensoes
        {
            public double LarguraOriginal { get; set; }
            public double AlturaOriginal { get; set; }
            public double PaddingOriginal { get; set; }

            public double LarguraProcessada { get; set; }
            public double AlturaProcessada { get; set; }
            public double PaddingProcessado { get; set; }
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

    public struct ConjuntoDadosDARV
    {
        public DadosEmitente Emitente { get; set; }
        public DadosCliente Cliente { get; set; }
        public string DataVenda { get; set; }
        public string IdVenda { get; set; }
        public string Barras { get; set; }
        public string ChaveNFeRelacionada { get; set; }
        public string Vendedor { get; set; }
        public string CPFVendedor { get; set; }
        public string Motorista { get; set; }
        public DadosProduto[] Produtos { get; set; }
        public string Desconto { get; set; }
        public string Total { get; set; }
        public string Observacoes { get; set; }
    }

    public struct DadosEmitente
    {
        public string NomeFicticio { get; set; }
        public PacotesImpressaoGenericos.Endereço Endereco { get; set; }
    }

    public struct DadosCliente
    {
        public string Nome { get; set; }
        public PacotesImpressaoGenericos.Endereço Endereco { get; set; }
    }

    public struct DadosProduto
    {
        public string Descricao { get; set; }
        public string Quantidade { get; set; }
        public string Valor { get; set; }
        public string Total { get; set; }
    }
}
