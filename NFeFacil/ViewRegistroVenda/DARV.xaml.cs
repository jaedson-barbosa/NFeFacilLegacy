using NFeFacil.ItensBD;
using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
        ConjuntoDadosDARV Dados { get; set; }
        double alturaDesejadaPagina;
        double larguraDesejadaPagina;
        int paddingPadrao = 1;
        double alturaUtil => alturaDesejadaPagina - (2 * paddingPadrao);
        double larguraUtil => larguraDesejadaPagina - (2 * paddingPadrao);

        public DARV()
        {
            this.InitializeComponent();
            DefinirTamanho(21, 29.7, 1);
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
                    Valor = x.Quantidade.ToString("C2"),
                    Total = x.Quantidade.ToString("C2")
                });

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
                    DataVenda = original.DataHoraVenda.ToString("dd-MM-yyyy HH:mm:ss"),
                    IdVenda = original.Id.ToString(),
                    ChaveNFeRelacionada = original.NotaFiscalRelacionada,
                    Vendedor = vendedor?.Nome ?? string.Empty,
                    CPFVendedor = vendedor?.CPF.ToString("000,000,000-00") ?? string.Empty,
                    Motorista = motorista?.Nome ?? string.Empty,
                    Produtos = produtos.ToArray(),
                    Desconto = original.DescontoTotal.ToString("C2"),
                    Total = original.DescontoTotal.ToString("C2"),
                    Observacoes = original.Observações
                };
            }
        }

        void DefinirTamanho(double largura, double altura, double padding)
        {
            txtLargura.Text = largura.ToString("F2");
            txtAltura.Text = altura.ToString("F2");

            var filhos = paiPaginas.Children;
            for (int i = 0; i < filhos.Count; i++)
            {
                var filho = (Grid)filhos[i];
                filho.Width = CentimeterToPixel(largura - 2);
                filho.Height = CentimeterToPixel(altura - 2);
                filho.Padding = new Thickness(CentimeterToPixel(padding));
            }
        }

        private void PaginaPrincipalCarregada(object sender, RoutedEventArgs e)
        {
            var altura = linhaProdutos.ActualHeight;
            int nItens1Pag = (int)((altura / 22) - 1);
            if (Dados.Produtos.Length <= nItens1Pag)
            {
                listaPrincipal.ItemsSource = Dados.Produtos.GerarObs();
            }
            else
            {
                listaPrincipal.ItemsSource = Dados.Produtos.Take(nItens1Pag).GerarObs();
                int nItens2Pag = (int)((alturaUtil / 22) - 1);
                var nPaginas = (Dados.Produtos.Length - nItens1Pag) / nItens2Pag;
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

            var grid = new Grid()
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1)
            };
            grid.Children.Add(lista);
            paiPaginas.Children.Add(grid);
        }
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
