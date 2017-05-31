using BibliotecaCentral.Log;
using BibliotecaCentral.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral.ItensBD;
using System.ComponentModel;
using System.Collections.ObjectModel;
using BibliotecaCentral.IBGE;
using System.Linq;
using BibliotecaCentral.ModeloXML;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarDestinatario : Page
    {
        private ClienteDI cliente;
        private TipoOperacao tipoRequisitado;
        private ILog Log = new Popup();

        public AdicionarDestinatario()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GrupoViewBanco<ClienteDI> parametro;
            if (e.Parameter == null)
            {
                parametro = new GrupoViewBanco<ClienteDI>
                {
                    ItemBanco = new ClienteDI(),
                    OperacaoRequirida = TipoOperacao.Adicao
                };
            }
            else
            {
                parametro = (GrupoViewBanco<ClienteDI>)e.Parameter;
            }
            cliente = parametro.ItemBanco;
            tipoRequisitado = parametro.OperacaoRequirida;
            switch (tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    MainPage.Current.SeAtualizar(Symbol.Add, "Adicionar cliente");
                    break;
                case TipoOperacao.Edicao:
                    MainPage.Current.SeAtualizar(Symbol.Edit, "Editar cliente");
                    break;
            }
            DataContext = new ClienteDataContext(ref cliente);
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (new ValidadorDestinatario(cliente).Validar(Log))
            {
                using (var db = new Clientes())
                {
                    if (tipoRequisitado == TipoOperacao.Adicao)
                    {
                        db.Adicionar(cliente);
                        Log.Escrever(TitulosComuns.Sucesso, "Cliente salvo com sucesso.");
                    }
                    else
                    {
                        db.Atualizar(cliente);
                        Log.Escrever(TitulosComuns.Sucesso, "Cliente alterado com sucesso.");
                    }
                }
                MainPage.Current.Retornar();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }

        private sealed class ClienteDataContext : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public ClienteDI Cliente { get; set; }

            public ObservableCollection<IndicadorIE> IndicadoresIE => BibliotecaCentral.Extensoes.ObterItens<IndicadorIE>();
            public IndicadorIE IndicadorIESelecionado
            {
                get => (IndicadorIE)Cliente.IndicadorIE;
                set
                {
                    Cliente.IndicadorIE = (int)value;
                    switch (value)
                    {
                        case IndicadorIE.Contribuinte:
                            IsentoICMS = false;
                            Cliente.InscricaoEstadual = string.Empty;
                            break;
                        case IndicadorIE.Isento:
                            IsentoICMS = true;
                            Cliente.InscricaoEstadual = null;
                            break;
                        case IndicadorIE.Não_Contribuinte:
                            IsentoICMS = true;
                            Cliente.InscricaoEstadual = null;
                            break;
                    }
                    PropertyChanged(this, new PropertyChangedEventArgs("IsentoICMS"));
                    PropertyChanged(this, new PropertyChangedEventArgs("InscricaoEstadual"));
                }
            }

            public bool IsentoICMS { get; set; }
            public string InscricaoEstadual
            {
                get => Cliente.InscricaoEstadual;
                set => Cliente.InscricaoEstadual = value;
            }

            public string UFEscolhida
            {
                get => Cliente.SiglaUF;
                set
                {
                    Cliente.SiglaUF = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
                }
            }

            public Municipio ConjuntoMunicipio
            {
                get
                {
                    var mun = Municipios.Get(Cliente.SiglaUF).FirstOrDefault(x => x.Codigo == Cliente.CodigoMunicipio);
                    return mun;
                }
                set
                {
                    Cliente.NomeMunicipio = value?.Nome;
                    Cliente.CodigoMunicipio = value?.Codigo ?? 0;
                }
            }

            private bool? nacional = null;
            public bool Nacional
            {
                get
                {
                    if (nacional == null)
                    {
                        var xpais = Cliente.XPais;
                        nacional = xpais.ToLower() == "brasil" || string.IsNullOrEmpty(xpais);
                    }
                    if (!nacional.Value)
                    {
                        Cliente.CEP = Cliente.SiglaUF = null;
                        ConjuntoMunicipio = null;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cliente)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConjuntoMunicipio)));
                    }
                    return nacional.Value;
                }
                set
                {
                    nacional = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nacional)));
                }
            }

            public int TipoDocumento { get; set; }

            public string Documento
            {
                get { return Cliente.Documento; }
                set
                {
                    var tipo = (TiposDocumento)TipoDocumento;
                    Cliente.CPF = tipo == TiposDocumento.CPF ? value : null;
                    Cliente.CNPJ = tipo == TiposDocumento.CNPJ ? value : null;
                    Cliente.IdEstrangeiro = tipo == TiposDocumento.idEstrangeiro ? value : null;
                }
            }

            public ClienteDataContext(ref ClienteDI dest)
            {
                TipoDocumento = (int)dest.TipoDocumento;
                Cliente = dest;
            }

            public enum IndicadorIE
            {
                Contribuinte = 1,
                Isento = 2,
                Não_Contribuinte = 9
            }
        }
    }
}
