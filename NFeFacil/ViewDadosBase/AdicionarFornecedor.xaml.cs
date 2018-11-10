using BaseGeral;
using BaseGeral.IBGE;
using BaseGeral.ItensBD;
using BaseGeral.Validacao;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarFornecedor : Page
    {
        FornecedorDI Fornecedor { get; set; }
        ObservableCollection<string> ListaMunicipios { get; set; }

        string UFEscolhida
        {
            get => Fornecedor.SiglaUF;
            set
            {
                Fornecedor.SiglaUF = value;
                ListaMunicipios.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    ListaMunicipios.Add(item.Nome);
                }
            }
        }

        public AdicionarFornecedor()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Fornecedor = (FornecedorDI)e.Parameter ?? new FornecedorDI();
            ListaMunicipios = string.IsNullOrEmpty(Fornecedor.SiglaUF)
                ? new ObservableCollection<string>()
                : new ObservableCollection<string>(Municipios.Get(Fornecedor.SiglaUF).Select(x => x.Nome));
        }

        void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidarDados(new ValidadorEndereco(Fornecedor)).ValidarTudo(true,
                    (string.IsNullOrEmpty(Fornecedor.Nome), "Não foi informado o nome do fornecedor"),
                    (string.IsNullOrEmpty(Fornecedor.CNPJ), "Não foi informado o CNPJ do fornecedor")))
                {
                    using (var repo = new BaseGeral.Repositorio.Escrita())
                    {
                        repo.SalvarItemSimples(Fornecedor, DefinicoesTemporarias.DateTimeNow);
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
