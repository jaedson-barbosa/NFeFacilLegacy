using NFeFacil.Log;
using NFeFacil.Validacao;
using Windows.UI.Xaml.Navigation;
using System;
using NFeFacil.ItensBD;
using System.Collections.ObjectModel;
using NFeFacil.IBGE;
using System.Linq;

namespace NFeFacil.ViewDadosBase
{
    internal sealed class BaseAdicaoDestinatario
    {
        internal ClienteDI Cliente { get; private set; }
        internal ObservableCollection<Municipio> ListaMunicipios { get; }
        readonly ILog Log = Popup.Current;

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
                if (new ValidadorDestinatario(Cliente).Validar(Log))
                {
                    using (var repo = new Repositorio.Escrita())
                    {
                        repo.SalvarCliente(Cliente, Propriedades.DateTimeNow);
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
