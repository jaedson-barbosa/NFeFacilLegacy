using System;
using System.Linq;
using BaseGeral.Validacao;
using BaseGeral.ItensBD;
using System.Collections.ObjectModel;
using BaseGeral.IBGE;
using BaseGeral;
using Windows.UI.Xaml;
using System.ComponentModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public abstract class ControllerAdicaoClienteGeral : INotifyPropertyChanged
    {
        public ClienteDI Cliente { get; }
        public ObservableCollection<Municipio> ListaMunicipios { get; }

        public string UFEscolhida
        {
            get => Cliente.SiglaUF;
            set
            {
                Cliente.SiglaUF = value;
                ListaMunicipios.Clear();
                foreach (var item in Municipios.Get(value))
                    ListaMunicipios.Add(item);
            }
        }

        public Municipio ConjuntoMunicipio
        {
            get => Municipios.Get(Cliente.SiglaUF).FirstOrDefault(x => x.Codigo == Cliente.CodigoMunicipio);
            set
            {
                Cliente.NomeMunicipio = value?.Nome;
                Cliente.CodigoMunicipio = value?.Codigo ?? 0;
            }
        }

        public abstract string Documento { get; set; }
        public virtual int IndicadorIESelecionado { get; set; }
        public readonly Visibility VisIndicadorIE, VisIE, VisISUF, VisEndNacional, VisEndEstrageiro;

        bool enabledIE = true;
        public bool EnabledIE
        {
            get => enabledIE;
            set
            {
                enabledIE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnabledIE"));
            }
        }

        public string IE
        {
            get => Cliente.InscricaoEstadual;
            set
            {
                Cliente.InscricaoEstadual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IE"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected ControllerAdicaoClienteGeral(ClienteDI cliente, int indicadorIE,
            Visibility visIndicadorIE, Visibility visIE, Visibility visISUF, Visibility visEndNacional, Visibility visEndEstrageiro)
        {
            VisIndicadorIE = visIndicadorIE;
            VisIE = visIE;
            VisISUF = visISUF;
            VisEndNacional = visEndNacional;
            VisEndEstrageiro = visEndEstrageiro;
            Cliente = cliente ?? new ClienteDI
            {
                IndicadorIE = indicadorIE,
                XPais = "BRASIL",
                CPais = 1058
            };
            ListaMunicipios = new ObservableCollection<Municipio>(Municipios.Get(Cliente.SiglaUF));
        }

        public void Confirmar()
        {
            try
            {
                if (new ValidarDados(new ValidadorEndereco(Cliente)).ValidarTudo(true,
                    (string.IsNullOrEmpty(Cliente.Nome), "Não foi informado o nome do cliente"),
                    (string.IsNullOrEmpty(Cliente.Documento), "Não foi informado nenhum documento de identificação do cliente")))
                {
                    using (var repo = new BaseGeral.Repositorio.Escrita())
                        repo.SalvarItemSimples(Cliente, DefinicoesTemporarias.DateTimeNow);
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        public void Cancelar() => MainPage.Current.Retornar();
    }
}