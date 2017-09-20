using NFeFacil.ItensBD;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        public VisualizacaoRegistroVenda()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.View, "Registro de venda");
            ItemBanco = (RegistroVenda)e.Parameter;
            using (var db = new AplicativoContext())
            {
                var cliente = db.Clientes.Find(ItemBanco.Cliente);
                var emitente = db.Emitentes.Find(ItemBanco.Emitente);
                var motorista = ItemBanco.Motorista != Guid.Empty ? db.Motoristas.Find(ItemBanco.Motorista) : null;
                var vendedor = ItemBanco.Vendedor != Guid.Empty ? db.Vendedores.Find(ItemBanco.Vendedor) : null;

                visualizacao.AddBloco("Emitente", ("Nome", emitente.Nome),
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

                visualizacao.AddBloco("Cliente", ("Nome", cliente.Nome),
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

                if (motorista != null)
                {
                    visualizacao.AddBloco("Motorista", ("Nome", motorista.Nome),
                        ("CNPJ", motorista.CNPJ),
                        ("CPF", motorista.CPF),
                        ("Inscrição Estadual", motorista.InscricaoEstadual),
                        ("Endereço", motorista.XEnder),
                        ("Municipio", motorista.XMun));
                }

                if (vendedor != null)
                {
                    visualizacao.AddBloco("Vendedor", ("Nome", vendedor.Nome),
                        ("CPF", vendedor.CPF.ToString("000,000,000-00")),
                        ("Endereço", vendedor.Endereço));
                }

                visualizacao.AddBloco("Outras informações", ("Observações", ItemBanco.Observações),
                    ("Data", ItemBanco.DataHoraVenda.ToString("dd-MM-yyyy")),
                    ("ID", ItemBanco.Id.ToString().ToUpper()),
                    ("NFe relacionada", ItemBanco.NotaFiscalRelacionada));

                visualizacao.AddBloco("Produtos", ItemBanco.Produtos.Select(x =>
                {
                    var label = db.Produtos.Find(x.IdBase).Descricao;
                    var texto = $"Quantidade - {x.Quantidade}, Total - {x.TotalLíquido}";
                    return (label, texto);
                }).ToArray());
            }
            btnCriarDarv.IsEnabled = btnCriarNFe.IsEnabled = btnCancelar.IsEnabled = !ItemBanco.Cancelado;
            btnVisualizarCancelamento.IsEnabled = ItemBanco.Cancelado;
        }

        private void CriarNFe(object sender, RoutedEventArgs e)
        {
            var nfe = ItemBanco.ToNFe();
            nfe.Informacoes.identificacao.DefinirVersãoAplicativo();
            MainPage.Current.Navegar<ViewNFe.ManipulacaoNotaFiscal>(nfe);
        }

        private void CriarDARV(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<DARV>(ItemBanco);
        }

        private async void Cancelar(object sender, RoutedEventArgs e)
        {
            var caixa = new MotivoCancelamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                using (var db = new AplicativoContext())
                {
                    ItemBanco.UltimaData = DateTime.Now;
                    ItemBanco.Cancelado = true;
                    db.Update(ItemBanco);
                    ItemBanco.Produtos.ForEach(x => x.DesregistrarAlteracaoEstoque(db));
                    var cancelamento = new CancelamentoRegistroVenda()
                    {
                        Motivo = caixa.Motivo,
                        MomentoCancelamento = DateTime.Now,
                        Id = ItemBanco.Id
                    };
                    db.CancelamentosRegistroVenda.Add(cancelamento);
                    db.SaveChanges();
                    btnCriarDarv.IsEnabled = btnCriarNFe.IsEnabled = btnCancelar.IsEnabled = false;
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
    }
}
