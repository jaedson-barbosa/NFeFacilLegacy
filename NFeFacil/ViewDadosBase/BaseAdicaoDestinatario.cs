using BaseGeral.Validacao;
using Windows.UI.Xaml.Navigation;
using System;
using BaseGeral.ItensBD;
using System.Collections.ObjectModel;
using BaseGeral.IBGE;
using System.Linq;
using BaseGeral;

namespace NFeFacil.ViewDadosBase
{
    internal sealed class BaseAdicaoDestinatario
    {
        internal ClienteDI Cliente { get; }
        internal ObservableCollection<Municipio> ListaMunicipios { get; }

        public string UFEscolhida
        {
            get => Cliente.SiglaUF;
            set
            {
                Cliente.SiglaUF = value;
                ListaMunicipios.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    ListaMunicipios.Add(item);
                }
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

        internal BaseAdicaoDestinatario(NavigationEventArgs e, bool nacional = true)
        {
            if (e.Parameter == null)
            {
                Cliente = new ClienteDI()
                {
                    IndicadorIE = 1
                };
                if (nacional)
                {
                    Cliente.XPais = "BRASIL";
                    Cliente.CPais = 1058;
                }
            }
            else
            {
                Cliente = (ClienteDI)e.Parameter;
            }
            ListaMunicipios = new ObservableCollection<Municipio>(Municipios.Get(Cliente.SiglaUF));
        }

        internal void Confirmar()
        {
            try
            {
                if (new ValidarDados(new ValidadorEndereco(Cliente)).ValidarTudo(true,
                    (string.IsNullOrEmpty(Cliente.Nome), "Não foi informado o nome do cliente"),
                    (string.IsNullOrEmpty(Cliente.Documento), "Não foi informado nenhum documento de identificação do cliente")))
                {
                    using (var repo = new BaseGeral.Repositorio.Escrita())
                    {
                        repo.SalvarItemSimples(Cliente, DefinicoesTemporarias.DateTimeNow);
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        internal void Cancelar()
        {
            MainPage.Current.Retornar();
        }
    }
}
