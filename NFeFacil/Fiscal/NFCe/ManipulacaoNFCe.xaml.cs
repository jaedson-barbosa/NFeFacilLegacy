using NFeFacil.Controles;
using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesDetalhes;
using NFeFacil.Produto;
using NFeFacil.Validacao;
using NFeFacil.View;
using NFeFacil.Fiscal.ViewNFe.CaixasDialogo;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal.ViewNFCe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoNFCe : Page, IHambuguer, IValida
    {
        const string NomeClienteHomologacao = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
        NFCe NotaSalva { get; set; } = new NFCe();
        public bool Concluido { get; set; }

        public ManipulacaoNFCe()
        {
            InitializeComponent();
        }

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Tag, "Identificação"),
            new ItemHambuguer(Symbol.People, "Cliente"),
            new ItemHambuguer(Symbol.Street, "Retirada/Entrega"),
            new ItemHambuguer(Symbol.Shop, "Produtos"),
            new ItemHambuguer("\uE806", "Motorista"),
            new ItemHambuguer(Symbol.Comment, "Informações adicionais")
        };

        public int SelectedIndex { set => main.SelectedIndex = value; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MunicipiosIdentificacao = Municipios.Get(NotaSalva.Informacoes.identificacao.CódigoUF).GerarObs();
        }

        string DataHoraEmissão
        {
            get => NotaSalva.Informacoes.identificacao.DataHoraEmissão;
            set => NotaSalva.Informacoes.identificacao.DataHoraEmissão = value;
        }

        public DateTimeOffset DataEmissao
        {
            get
            {
                if (string.IsNullOrEmpty(DataHoraEmissão))
                {
                    var agora = DefinicoesTemporarias.DateTimeNow;
                    DataHoraEmissão = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(DataHoraEmissão);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(DataHoraEmissão);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraEmissao
        {
            get => DataEmissao.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(DataHoraEmissão);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public int FinalidadeEmissao
        {
            get => NotaSalva.Informacoes.identificacao.FinalidadeEmissao - 1;
            set => NotaSalva.Informacoes.identificacao.FinalidadeEmissao = value + 1;
        }

        ObservableCollection<Municipio> MunicipiosIdentificacao { get; set; }

        public int CodigoMunicipio
        {
            get => NotaSalva.Informacoes.identificacao.CodigoMunicipio;
            set => NotaSalva.Informacoes.identificacao.CodigoMunicipio = value;
        }

        ClienteDI[] TodosClientes;
        ObservableCollection<ClienteDI> ClientesDisponiveis { get; set; }
        ProdutoDI[] TodosProdutos;
        ObservableCollection<ProdutoDI> ProdutosDisponiveis { get; set; }
        MotoristaDI[] TodosMotoristas;
        ObservableCollection<MotoristaDI> MotoristasDisponiveis { get; set; }

        ClienteDI clienteSelecionado;
        ClienteDI ClienteSelecionado
        {
            get
            {
                var dest = NotaSalva.Informacoes.destinatário;
                if (clienteSelecionado == null && dest != null)
                {
                    clienteSelecionado = ClientesDisponiveis.FirstOrDefault(x => x.Documento == dest.Documento);
                }
                return clienteSelecionado;
            }
            set
            {
                clienteSelecionado = value;
                NotaSalva.Informacoes.destinatário = value.ToDestinatario();
                if (NotaSalva.AmbienteTestes)
                {
                    NotaSalva.Informacoes.destinatário.Nome = NomeClienteHomologacao;
                }
            }
        }

        MotoristaDI motoristaSelecionado;
        MotoristaDI MotoristaSelecionado
        {
            get
            {
                var mot = NotaSalva.Informacoes.transp?.Transporta;
                if (motoristaSelecionado.Equals(default(MotoristaDI)) && mot?.Documento != null)
                {
                    motoristaSelecionado = MotoristasDisponiveis.FirstOrDefault(x => x.Documento == mot.Documento);
                }
                return motoristaSelecionado;
            }
            set
            {
                motoristaSelecionado = value;
                NotaSalva.Informacoes.transp.Transporta = value.ToMotorista();
            }
        }

        ObservableCollection<DetalhesProdutos> Produtos { get; set; }

        void AdicionarProduto(object sender, ItemClickEventArgs e)
        {
            var prod = (ProdutoDI)e.ClickedItem;
            var dados = new DadosAdicaoProduto(prod);
            MainPage.Current.Navegar<ProdutoNFCe>(dados);
        }

        void EditarProduto(DetalhesProdutos produto)
        {
            using (var repo = new Repositorio.Leitura())
            {
                var prodDI = repo.ObterProduto(produto.Produto.CodigoProduto);
                var dados = new DadosAdicaoProduto(prodDI, produto);
                MainPage.Current.Navegar<ProdutoNFCe>(dados);
            }
        }

        void RemoverProduto(DetalhesProdutos produto)
        {
            NotaSalva.Informacoes.produtos.Remove(produto);
            Produtos.Remove(produto);
            NotaSalva.Informacoes.total = new Total(NotaSalva.Informacoes.produtos);
        }

        private async void AlterarEnderecoRetirada(object sender, RoutedEventArgs e)
        {
            var controle = (ToggleSwitch)sender;
            if (controle.IsOn)
            {
                var caixa = new EscolherTipoEndereco();
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    var tipo = caixa.TipoEscolhido;
                    if (!caixa.Nacional)
                    {
                        var caixa2 = new EnderecoDiferenteExterior();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            NotaSalva.Informacoes.Retirada = caixa2.Endereco;
                        }
                    }
                    else
                    {
                        var caixa2 = new EnderecoDiferenteNacional();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            NotaSalva.Informacoes.Retirada = caixa2.Endereco;
                        }
                    }
                }
            }
            else
            {
                NotaSalva.Informacoes.Retirada = null;
            }

            if (controle.IsOn && NotaSalva.Informacoes.Retirada == null)
            {
                controle.IsOn = false;
            }
        }

        private async void AlterarEnderecoEntrega(object sender, RoutedEventArgs e)
        {
            var controle = (ToggleSwitch)sender;
            if (controle.IsOn)
            {
                var caixa = new EscolherTipoEndereco();
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    var tipo = caixa.TipoEscolhido;
                    if (!caixa.Nacional)
                    {
                        var caixa2 = new EnderecoDiferenteExterior();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            NotaSalva.Informacoes.Entrega = caixa2.Endereco;
                        }
                    }
                    else
                    {
                        var caixa2 = new EnderecoDiferenteNacional();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            NotaSalva.Informacoes.Entrega = caixa2.Endereco;
                        }
                    }
                }
            }
            else
            {
                NotaSalva.Informacoes.Entrega = null;
            }

            if (controle.IsOn && NotaSalva.Informacoes.Entrega == null)
            {
                controle.IsOn = false;
            }
        }

        private void GridView_Loaded(object sender, RoutedEventArgs e)
        {
            var input = (GridView)sender;
            var cliente = input.SelectedItem;
            if (cliente != null)
            {
                input.ScrollIntoView(cliente, ScrollIntoViewAlignment.Leading);
            }
        }

        private void BuscarCliente(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < TodosClientes.Length; i++)
            {
                var atual = TodosClientes[i];
                bool valido = (DefinicoesPermanentes.ModoBuscaCliente == 0
                    ? atual.Nome : atual.Documento).ToUpper().Contains(busca.ToUpper());
                if (valido && !ClientesDisponiveis.Contains(atual))
                {
                    ClientesDisponiveis.Add(atual);
                }
                else if (!valido && ClientesDisponiveis.Contains(atual))
                {
                    ClientesDisponiveis.Remove(atual);
                }
            }
        }

        private void BuscarMotorista(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < TodosMotoristas.Length; i++)
            {
                var atual = TodosMotoristas[i];
                bool valido = (DefinicoesPermanentes.ModoBuscaMotorista == 0
                    ? atual.Nome : atual.Documento).ToUpper().Contains(busca.ToUpper());
                if (valido && !MotoristasDisponiveis.Contains(atual))
                {
                    MotoristasDisponiveis.Add(atual);
                }
                else if (!valido && MotoristasDisponiveis.Contains(atual))
                {
                    MotoristasDisponiveis.Remove(atual);
                }
            }
        }

        private void BuscarProduto(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < TodosProdutos.Length; i++)
            {
                var atual = TodosProdutos[i];
                bool valido = (DefinicoesPermanentes.ModoBuscaProduto == 0
                    ? atual.Descricao : atual.CodigoProduto).ToUpper().Contains(busca.ToUpper());
                if (valido && !ProdutosDisponiveis.Contains(atual))
                {
                    ProdutosDisponiveis.Add(atual);
                }
                else if (!valido && ProdutosDisponiveis.Contains(atual))
                {
                    ProdutosDisponiveis.Remove(atual);
                }
            }
        }

        void Confirmar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NotaSalva.AmbienteTestes)
                {
                    NotaSalva.Informacoes.destinatário.Nome = NomeClienteHomologacao;
                }

                var ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
                if (ultPage.SourcePageType == typeof(ViewRegistroVenda.VisualizacaoRegistroVenda))
                {
                    Frame.BackStack.Remove(ultPage);
                    ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
                }

                var nota = NotaSalva;
                new AnalisadorNFCe(ref nota).Normalizar();

                using (var repo = new Repositorio.OperacoesExtras())
                {
                    string IDOriginal = nota.Informacoes.Id;
                    nota.Informacoes.AtualizarChave();
                    string NovoId = nota.Informacoes.Id;

                    repo.ProcessarNFeLocal(IDOriginal, NovoId);
                }

                if (ultPage.SourcePageType != typeof(VisualizacaoNFe))
                {
                    var novoDI = new NFeDI(nota, nota.ToXElement().ToString())
                    {
                        Status = (int)StatusNFe.Validada
                    };
                    Frame.BackStack.Add(new PageStackEntry(typeof(VisualizacaoNFe), novoDI, null));
                }
                else
                {
                    var di = (NFeDI)ultPage.Parameter;
                    di.Id = nota.Informacoes.Id;
                    di.NomeCliente = nota.Informacoes.destinatário.Nome;
                    di.DataEmissao = DateTime.Parse(nota.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
                    di.Status = (int)StatusNFe.Validada;
                    di.XML = nota.ToXElement().ToString();
                }

                Concluido = true;
                MainPage.Current.Retornar();
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }
    }
}
