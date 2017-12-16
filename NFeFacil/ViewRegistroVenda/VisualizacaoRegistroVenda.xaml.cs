using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class VisualizacaoRegistroVenda : Page
    {
        RegistroVenda ItemBanco;
        ClienteDI cliente;
        MotoristaDI motorista;
        Vendedor vendedor;
        Comprador comprador;
        ProdutoDI[] produtosCompletos;

        public VisualizacaoRegistroVenda()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ItemBanco = (RegistroVenda)e.Parameter;
            using (var db = new AplicativoContext())
            {
                cliente = db.Clientes.Find(ItemBanco.Cliente);
                motorista = ItemBanco.Motorista != Guid.Empty ? db.Motoristas.Find(ItemBanco.Motorista) : null;
                vendedor = ItemBanco.Vendedor != Guid.Empty ? db.Vendedores.Find(ItemBanco.Vendedor) : null;
                comprador = ItemBanco.Comprador != Guid.Empty ? db.Compradores.Find(ItemBanco.Comprador) : null;
                produtosCompletos = db.Produtos.Where(x => ItemBanco.Produtos.Count(y => x.Id == y.IdBase) > 0).ToArray();
                var emitente = Propriedades.EmitenteAtivo;

                AddBloco("Emitente", ("Nome", emitente.Nome),
                    ("Nome fantasia", emitente.NomeFantasia),
                    ("Bairro", emitente.Bairro),
                    ("CEP", emitente.CEP),
                    ("CNPJ", emitente.CNPJ),
                    ("Complemento", emitente.Complemento),
                    ("Inscrição Estadual", emitente.InscricaoEstadual),
                    ("Logradouro", emitente.Logradouro),
                    ("Municipio", emitente.NomeMunicipio),
                    ("Numero", emitente.Numero),
                    ("UF", emitente.SiglaUF),
                    ("Telefone", emitente.Telefone));

                AddBloco("Cliente", ("Nome", cliente.Nome),
                    ("Bairro", cliente.Bairro),
                    ("CEP", cliente.CEP),
                    ("CNPJ", cliente.CNPJ),
                    ("CPF", cliente.CPF),
                    ("Complemento", cliente.Complemento),
                    ("Inscrição Estadual", cliente.InscricaoEstadual),
                    ("Logradouro", cliente.Logradouro),
                    ("Municipio", cliente.NomeMunicipio),
                    ("Numero", cliente.Numero),
                    ("UF", cliente.SiglaUF),
                    ("Telefone", cliente.Telefone));

                if (comprador != null)
                {
                    AddBloco("Comprador", ("Nome", comprador.Nome),
                        ("Telefone", comprador.Telefone),
                        ("Email", comprador.Email));
                }

                if (motorista != null)
                {
                    AddBloco("Motorista", ("Nome", motorista.Nome),
                        ("CNPJ", motorista.CNPJ),
                        ("CPF", motorista.CPF),
                        ("Inscrição Estadual", motorista.InscricaoEstadual),
                        ("Endereço", motorista.XEnder),
                        ("Municipio", motorista.XMun));
                }

                if (vendedor != null)
                {
                    AddBloco("Vendedor", ("Nome", vendedor.Nome),
                        ("CPF", ExtensoesPrincipal.AplicarMáscaraDocumento(vendedor.CPFStr)),
                        ("Endereço", vendedor.Endereço));
                }

                AddBloco("Outras informações", ("Observações", ItemBanco.Observações),
                    ("Data", ItemBanco.DataHoraVenda.ToString("dd-MM-yyyy")),
                    ("ID", ItemBanco.Id.ToString().ToUpper()),
                    ("NFe relacionada", ItemBanco.NotaFiscalRelacionada));

                AddBloco("Produtos", ItemBanco.Produtos.Select(x =>
                {
                    var label = produtosCompletos.First(k => k.Id == x.IdBase).Descricao;
                    var texto = $"Quantidade - {x.Quantidade}, Total - {x.TotalLíquido}";
                    return (label, texto);
                }).ToArray());
            }
            var semNota = string.IsNullOrEmpty(ItemBanco.NotaFiscalRelacionada);
            btnCriarNFe.IsEnabled = semNota;
            btnVerNFe.IsEnabled = !semNota;
            btnCriarDarv.IsEnabled = btnCriarNFe.IsEnabled =
                btnCancelar.IsEnabled = btnCalcularTroco.IsEnabled = !ItemBanco.Cancelado;
            btnVisualizarCancelamento.IsEnabled = ItemBanco.Cancelado;
        }

        public void AddBloco(string titulo, params (string, string)[] filhos)
        {
            const string EntreLabelTexto = ": ";
            var paragrafo = new Paragraph();
            AddInline(titulo, Estilo.TituloBloco);
            for (int i = 0; i < filhos.Length; i++)
            {
                var atual = filhos[i];
                if (!string.IsNullOrEmpty(atual.Item2))
                {
                    AddInline(atual.Item1 + EntreLabelTexto, Estilo.Label);
                    AddInline(atual.Item2, Estilo.Texto);
                }
            }
            visualizacao.Blocks.Add(paragrafo);

            void AddInline(string texto, Estilo estilo)
            {
                var run = new Run() { Text = texto };
                switch (estilo)
                {
                    case Estilo.TituloBloco:
                        run.FontSize = 16;
                        run.FontWeight = FontWeights.ExtraBlack;
                        break;
                    case Estilo.Label:
                        run.FontWeight = FontWeights.Bold;
                        break;
                }
                paragrafo.Inlines.Add(run);
                if (estilo != Estilo.Label)
                {
                    CriarQuebraDeLinha();
                }
            }

            void CriarQuebraDeLinha()
            {
                paragrafo.Inlines.Add(new LineBreak());
            }
        }

        enum Estilo
        {
            TituloBloco,
            Label,
            Texto
        }

        private async void CriarNFe(object sender, RoutedEventArgs e)
        {
            var caixa = new ViewNFe.CriadorNFe(ItemBanco.ToNFe());
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                Log.Popup.Current.Escrever(Log.TitulosComuns.Atenção, "Os impostos dos produtos não são adicionados automaticamente, por favor, insira-os editando cada produto.");
            }
        }

        async void CriarDARV(object sender, RoutedEventArgs e)
        {
            var caixa = new EscolherDimensão();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                double largura = caixa.Largura, altura = caixa.Predefinicao == 0 ? 0 : caixa.Altura;

                MainPage.Current.Navegar<DARV>(new DadosImpressaoDARV
                {
                    Venda = ItemBanco,
                    Dimensoes = new Dimensoes(largura, altura, 1),
                    Cliente = cliente,
                    Motorista = motorista,
                    Vendedor = vendedor,
                    Comprador = comprador,
                    ProdutosCompletos = produtosCompletos
                });
            }
        }

        private async void Cancelar(object sender, RoutedEventArgs e)
        {
            var caixa = new MotivoCancelamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                using (var db = new AplicativoContext())
                {
                    ItemBanco.UltimaData = Propriedades.DateTimeNow;
                    ItemBanco.Cancelado = true;
                    db.Update(ItemBanco);

                    for (int i = 0; i < ItemBanco.Produtos.Count; i++)
                    {
                        var produto = ItemBanco.Produtos[i];
                        var estoque = db.Estoque.Include(x => x.Alteracoes).FirstOrDefault(x => x.Id == produto.IdBase);
                        if (estoque != null)
                        {
                            estoque.UltimaData = Propriedades.DateTimeNow;
                            estoque.Alteracoes.Add(new AlteracaoEstoque
                            {
                                Alteração = produto.Quantidade,
                                MomentoRegistro = Propriedades.DateTimeNow
                            });
                            db.Estoque.Update(estoque);
                        }
                    }

                    var cancelamento = new CancelamentoRegistroVenda()
                    {
                        Motivo = caixa.Motivo,
                        MomentoCancelamento = Propriedades.DateTimeNow,
                        Id = ItemBanco.Id
                    };
                    db.CancelamentosRegistroVenda.Add(cancelamento);

                    db.SaveChanges();
                    btnCriarDarv.IsEnabled = btnCriarNFe.IsEnabled
                        = btnCancelar.IsEnabled = btnCalcularTroco.IsEnabled = false;
                    btnVisualizarCancelamento.IsEnabled = true;
                }
            }
        }

        private async void VisualizarCancelamento(object sender, RoutedEventArgs e)
        {
            using (var db = new AplicativoContext())
            {
                var item = db.CancelamentosRegistroVenda.Find(ItemBanco.Id);
                await new VisualizarDetalhesCancelamento(item).ShowAsync();
            }
        }

        private void VerNFe(object sender, RoutedEventArgs e)
        {
            using (var db = new AplicativoContext())
            {
                var item = db.NotasFiscais.Find(ItemBanco.NotaFiscalRelacionada);
                MainPage.Current.Navegar<ViewNFe.VisualizacaoNFe>(item);
            }
        }

        async void CalcularTroco(object sender, RoutedEventArgs e)
        {
            var total = ItemBanco.Produtos.Sum(x => x.TotalLíquido);
            await new CalcularTroco(total).ShowAsync();
        }
    }
}
