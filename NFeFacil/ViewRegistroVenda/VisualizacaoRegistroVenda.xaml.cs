using NFeFacil.ItensBD;
using System;
using System.Linq;
using Windows.UI.Text;
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

        public VisualizacaoRegistroVenda()
        {
            this.InitializeComponent();
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
                        ("CPF", vendedor.CPF.ToString("000,000,000-00")),
                        ("Endereço", vendedor.Endereço));
                }

                AddBloco("Outras informações", ("Observações", ItemBanco.Observações),
                    ("Data", ItemBanco.DataHoraVenda.ToString("dd-MM-yyyy")),
                    ("ID", ItemBanco.Id.ToString().ToUpper()),
                    ("NFe relacionada", ItemBanco.NotaFiscalRelacionada));

                AddBloco("Produtos", ItemBanco.Produtos.Select(x =>
                {
                    var label = db.Produtos.Find(x.IdBase).Descricao;
                    var texto = $"Quantidade - {x.Quantidade}, Total - {x.TotalLíquido}";
                    return new ElementoBase { Label = label, Texto = texto };
                }).ToArray());
            }
        }

        void AddBloco(string titulo, params ElementoBase[] filhos)
        {
            const string EntreLabelTexto = ": ";
            var paragrafo = new Paragraph();
            AddInline(titulo, Estilo.TituloBloco);
            for (int i = 0; i < filhos.Length; i++)
            {
                var atual = filhos[i];
                if (!string.IsNullOrEmpty(atual.Texto))
                {
                    AddInline(atual.Label + EntreLabelTexto, Estilo.Label);
                    AddInline(atual.Texto, Estilo.Texto);
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

        struct ElementoBase
        {
            public string Label { get; set; }
            public string Texto { get; set; }

            public static implicit operator ElementoBase((string, string) k)
            {
                return new ElementoBase() { Label = k.Item1, Texto = k.Item2 };
            }
        }

        private void Editar(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainPage.Current.Navegar<ManipulacaoRegistroVenda>(ItemBanco);
        }

        private void CriarNFe(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var nfe = new ConjuntoManipuladorNFe
            {
                NotaSalva = ItemBanco.ToNFe(),
                StatusAtual = StatusNFe.Edição,
                OnNotaSalva = x =>
                {
                    ItemBanco.NotaFiscalRelacionada = x;
                }
            };
            nfe.NotaSalva.Informações.identificação.DefinirVersãoAplicativo();
            MainPage.Current.Navegar<View.ManipulacaoNotaFiscal>(nfe);
        }

        private void CriarDARV(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainPage.Current.Navegar<DARV>(ItemBanco);
        }
    }
}
