using NFeFacil.Log;
using NFeFacil.Validacao;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NFeFacil.ItensBD;
using NFeFacil.IBGE;
using System.Collections.ObjectModel;
using System.Linq;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarEmitente : Page
    {
        EmitenteDI Emit { get; set; }
        string RegimeTributario
        {
            get => Emit.RegimeTributario.ToString();
            set => Emit.RegimeTributario = int.Parse(value);
        }

        string EstadoSelecionado
        {
            get => Emit.SiglaUF;
            set
            {
                Emit.SiglaUF = value;
                ListaMunicipios.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    ListaMunicipios.Add(item);
                }
            }
        }

        ObservableCollection<Municipio> ListaMunicipios { get; set; }
        Municipio ConjuntoMunicipio
        {
            get => Municipios.Get(Emit.SiglaUF).FirstOrDefault(x => x.Codigo == Emit.CodigoMunicipio);
            set
            {
                Emit.NomeMunicipio = value?.Nome;
                Emit.CodigoMunicipio = value?.Codigo ?? 0;
            }
        }

        ILog Log = Popup.Current;

        public AdicionarEmitente()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                Emit = new EmitenteDI();
            }
            else
            {
                Emit = (EmitenteDI)e.Parameter;
            }
            ListaMunicipios = new ObservableCollection<Municipio>(Municipios.Get(Emit.SiglaUF));
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidadorEmitente(Emit).Validar(Log))
                {
                    using (var repo = new Repositorio.MEGACLASSE())
                    {
                        repo.SalvarEmitente(Emit, Propriedades.DateTimeNow);
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
