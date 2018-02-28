using NFeFacil.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using NFeFacil.ItensBD;
using System.Collections.ObjectModel;
using NFeFacil.IBGE;
using NFeFacil.ModeloXML;
using System.Linq;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [View.DetalhePagina("\uE806", "Motorista")]
    public sealed partial class AdicionarMotorista : Page
    {
        MotoristaDI Motorista { get; set; }
        ObservableCollection<VeiculoDI> Veiculos { get; }
        ObservableCollection<string> ListaMunicipios { get; set; }

        string UFEscolhida
        {
            get => Motorista.UF;
            set
            {
                Motorista.UF = value;
                ListaMunicipios.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    ListaMunicipios.Add(item.Nome);
                }
            }
        }

        public bool IsentoICMS
        {
            get => Motorista.InscricaoEstadual == "ISENTO";
            set
            {
                Motorista.InscricaoEstadual = value ? "ISENTO" : null;
                txtIE.IsEnabled = !value;
            }
        }

        public int TipoDocumento { get; set; }
        public string Documento
        {
            get => Motorista.Documento;
            set
            {
                var tipo = (TiposDocumento)TipoDocumento;
                Motorista.CPF = tipo == TiposDocumento.CPF ? value : null;
                Motorista.CNPJ = tipo == TiposDocumento.CNPJ ? value : null;
            }
        }

        public AdicionarMotorista()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                Veiculos = repo.ObterVeiculos().GerarObs();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                Motorista = new MotoristaDI();
                ListaMunicipios = new ObservableCollection<string>();
            }
            else
            {
                Motorista = (MotoristaDI)e.Parameter;
                ListaMunicipios = new ObservableCollection<string>(Municipios.Get(Motorista.UF).Select(x => x.Nome));
            }
            TipoDocumento = (int)Motorista.TipoDocumento;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidarDados().ValidarTudo(true,
                    (string.IsNullOrEmpty(Motorista.UF), "Não foi definido uma UF"),
                    (string.IsNullOrEmpty(Motorista.XMun), "Não foi definido um município"),
                    (string.IsNullOrEmpty(Motorista.Nome), "Não foi informado o nome do motorista")))
                {
                    using (var repo = new Repositorio.Escrita())
                    {
                        Motorista.VeiculosSecundarios = string.Concat(from VeiculoDI item in grdVeisSec.SelectedItems
                                                                      select item.Placa + '&');
                        repo.SalvarItemSimples(Motorista, DefinicoesTemporarias.DateTimeNow);
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

        async void AdicionarVeiculo(object sender, RoutedEventArgs e)
        {
            var caixa = new AdicionarVeiculo();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var veic = caixa.Item;
                using (var repo = new Repositorio.Escrita())
                {
                    repo.SalvarItemSimples(veic, DefinicoesTemporarias.DateTimeNow);
                    Veiculos.Add(veic);
                }
            }
        }

        void grdVeisSec_Loaded(object sender, RoutedEventArgs e)
        {
            if (Motorista.VeiculosSecundarios?.Length > 0)
            {
                var veics = Motorista.VeiculosSecundarios.Split('&');
                foreach (var item in veics)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        grdVeisSec.SelectedItems.Add(Veiculos.First(x => x.Placa == item));
                    }
                }
            }
        }

        async void EditarVeiculo(object sender, RoutedEventArgs e)
        {
            var veic = (VeiculoDI)((FrameworkElement)sender).DataContext;
            var index = Veiculos.IndexOf(veic);
            var caixa = new AdicionarVeiculo(veic);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                veic = caixa.Item;
                using (var repo = new Repositorio.Escrita())
                {
                    repo.SalvarItemSimples(veic, DefinicoesTemporarias.DateTimeNow);
                    Veiculos.RemoveAt(index);
                    Veiculos.Insert(index, veic);
                }
            }
        }

        private void InativarVeiculo(object sender, RoutedEventArgs e)
        {
            var veic = (VeiculoDI)((FrameworkElement)sender).DataContext;
            using (var repo = new Repositorio.Escrita())
            {
                repo.InativarDadoBase(veic, DefinicoesTemporarias.DateTimeNow);
            }
            Veiculos.Remove(veic);
        }
    }
}
