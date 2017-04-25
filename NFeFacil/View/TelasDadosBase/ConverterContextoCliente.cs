using BibliotecaCentral.IBGE;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed class ConverterContextoCliente : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Destinatario clienteSimples)
            {
                return new ClienteDataContext(clienteSimples);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var contexto = (ClienteDataContext)value;
            if (targetType == typeof(Destinatario))
            {
                return contexto.Cliente;
            }
            throw new ArgumentException();
        }

        private sealed class ClienteDataContext : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public Destinatario Cliente { get; set; }

            public bool IsentoICMS
            {
                get => Cliente.indicadorIE != 1;
                set
                {
                    Cliente.indicadorIE = value ? 2 : 1;
                    Cliente.inscricaoEstadual = value ? "ISENTO" : null;
                    PropertyChanged(this, new PropertyChangedEventArgs("IsentoICMS"));
                    PropertyChanged(this, new PropertyChangedEventArgs("InscricaoEstadual"));
                }
            }

            public string InscricaoEstadual
            {
                get => Cliente.inscricaoEstadual;
                set => Cliente.inscricaoEstadual = value;
            }

            public string UFEscolhida
            {
                get => Cliente.endereco.SiglaUF;
                set
                {
                    Cliente.endereco.SiglaUF = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ConjuntoMunicipio"));
                }
            }

            public Municipio ConjuntoMunicipio
            {
                get
                {
                    var mun = Municipios.Get(Cliente.endereco.SiglaUF).FirstOrDefault(x => x.Codigo == Cliente.endereco.CodigoMunicipio);
                    return mun;
                }
                set
                {
                    Cliente.endereco.NomeMunicipio = value?.Nome;
                    Cliente.endereco.CodigoMunicipio = value?.Codigo ?? 0;
                }
            }

            private bool? nacional = null;
            public bool Nacional
            {
                get
                {
                    if (nacional == null)
                    {
                        var xpais = Cliente.endereco.XPais;
                        nacional = xpais.ToLower() == "brasil" || string.IsNullOrEmpty(xpais);
                    }
                    if (!nacional.Value)
                    {
                        Cliente.endereco.CEP = Cliente.endereco.SiglaUF = null;
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
                    Cliente.idEstrangeiro = tipo == TiposDocumento.idEstrangeiro ? value : null;
                }
            }

            public ClienteDataContext(Destinatario dest)
            {
                TipoDocumento = (int)dest.obterTipoDocumento;
                Cliente = dest;
            }
        }
    }
}
