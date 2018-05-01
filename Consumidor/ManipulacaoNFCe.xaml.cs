using BaseGeral.IBGE;
using BaseGeral.ItensBD;
using BaseGeral.ModeloXML;
using BaseGeral.Validacao;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BaseGeral;
using BaseGeral.View;
using Fiscal;
using System.ComponentModel;
using BaseGeral.Buscador;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Consumidor
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    [DetalhePagina(Symbol.Document, "Nota fiscal do consumidor")]
    public sealed partial class ManipulacaoNFCe : Page, INotifyPropertyChanged
    {
        NFCe NotaSalva { get; set; }
        Visibility VisibilitadeTransportador { get; set; }
        BuscadorCliente Clientes { get; }
        BuscadorMotorista Motoristas { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ManipulacaoNFCe()
        {
            InitializeComponent();
            VisibilitadeTransportador = Visibility.Collapsed;
            Clientes = new BuscadorCliente();
            Motoristas = new BuscadorMotorista();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NotaSalva = (NFCe)e.Parameter;
            MunicipiosIdentificacao = Municipios.Get(NotaSalva.Informacoes.identificacao.CódigoUF).GerarObs();
            FormasPagamento = NotaSalva.Informacoes.FormasPagamento.Select(x => new FormaPagamento(x)).GerarObs();
        }

        string IndicadorPresenca
        {
            get => NotaSalva.Informacoes.identificacao.IndicadorPresenca.ToString();
            set
            {
                var parsedValue = ushort.Parse(value);
                NotaSalva.Informacoes.identificacao.IndicadorPresenca = parsedValue;
                Visibility antiga = VisibilitadeTransportador, nova = (Visibility)(parsedValue == 4 ? 0 : 1);
                if (antiga != nova)
                {
                    VisibilitadeTransportador = nova;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilitadeTransportador)));
                }
            }
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

        ClienteDI clienteSelecionado;
        ClienteDI ClienteSelecionado
        {
            get
            {
                var dest = NotaSalva.Informacoes.destinatário;
                if (clienteSelecionado == null && dest != null)
                {
                    clienteSelecionado = Clientes.BuscarViaDocumento(dest.Documento);
                }
                return clienteSelecionado;
            }
            set
            {
                clienteSelecionado = value;
                NotaSalva.Informacoes.destinatário = value?.ToDestinatario();
            }
        }

        MotoristaDI motoristaSelecionado;
        MotoristaDI MotoristaSelecionado
        {
            get
            {
                var mot = NotaSalva.Informacoes.transp?.Transporta;
                if (motoristaSelecionado == null && mot?.Documento != null)
                {
                    motoristaSelecionado = Motoristas.BuscarViaDocumento(mot.Documento);
                }
                return motoristaSelecionado;
            }
            set
            {
                motoristaSelecionado = value;
                NotaSalva.Informacoes.transp.Transporta = value?.ToMotorista();
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
            Clientes.Buscar(busca);
        }

        private void BuscarMotorista(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Motoristas.Buscar(busca);
        }

        void Confirmar(object sender, RoutedEventArgs e)
        {
            try
            {
                var ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
                if (ultPage.SourcePageType.Name == "VisualizacaoRegistroVenda")
                {
                    Frame.BackStack.Remove(ultPage);
                    ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
                }
                Frame.BackStack.Remove(ultPage);
                ultPage = Frame.BackStack[Frame.BackStack.Count - 1];

                var nota = NotaSalva;
                new AnalisadorNFCe(ref nota).Normalizar();

                using (var repo = new BaseGeral.Repositorio.OperacoesExtras())
                {
                    string IDOriginal = nota.Informacoes.Id;
                    nota.Informacoes.AtualizarChave();
                    string NovoId = nota.Informacoes.Id;

                    repo.ProcessarNFeLocal(IDOriginal, NovoId);
                }

                if (ultPage.SourcePageType != typeof(Visualizacao))
                {
                    var novoDI = new NFeDI(nota, nota.ToXElement().ToString())
                    {
                        Status = (int)StatusNota.Validada
                    };
                    var acoes = new AcoesNFCe(novoDI, nota);
                    Frame.BackStack.Add(new PageStackEntry(typeof(Visualizacao), acoes, null));
                }
                else
                {
                    var acoes = (AcoesNFCe)ultPage.Parameter;
                    var di = acoes.ItemBanco;
                    di.Id = nota.Informacoes.Id;
                    di.NomeCliente = nota.Informacoes.destinatário?.Nome ?? "Desconhecido";
                    di.DataEmissao = DateTime.Parse(nota.Informacoes.identificacao.DataHoraEmissão).ToString("yyyy-MM-dd HH:mm:ss");
                    di.Status = (int)StatusNota.Validada;
                    di.XML = nota.ToXElement().ToString();
                }

                BasicMainPage.Current.Retornar();
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        ObservableCollection<FormaPagamento> FormasPagamento { get; set; }

        async void AdicionarFormaPagamento(object sender, RoutedEventArgs e)
        {
            var caixa = new AddFormaPagamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informacoes.FormasPagamento.Add(caixa.Pagamento);
                FormasPagamento.Add(new FormaPagamento(caixa.Pagamento));
            }
        }

        private void RemoverFormaPagamento(object sender, RoutedEventArgs e)
        {
            var forma = (FormaPagamento)((FrameworkElement)sender).DataContext;
            NotaSalva.Informacoes.FormasPagamento.Remove(forma.Original);
            FormasPagamento.Remove(forma);
        }

        void Voltar(object sender, RoutedEventArgs e)
        {
            var ultPage = Frame.BackStack[Frame.BackStack.Count - 1];
            var controle = (ControleViewProduto)ultPage.Parameter;
            controle.AtualizarControle(NotaSalva);
            controle.Voltar();
        }
    }
}
