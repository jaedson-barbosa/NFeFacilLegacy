using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed class ConverterContextoMotorista : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is MotoristaDI motorista)
            {
                return new MotoristaDataContext(motorista);
            }
            else if (value is Motorista motoristaSimples)
            {
                return new MotoristaDataContext(motoristaSimples);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var contexto = (MotoristaDataContext)value;
            if (targetType == typeof(MotoristaDI))
            {
                return contexto.Motorista as MotoristaDI;
            }
            else if (targetType == typeof(Motorista))
            {
                return contexto.Motorista;
            }
            throw new ArgumentException();
        }

        private sealed class MotoristaDataContext : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private Motorista motorista;
            public Motorista Motorista
            {
                get => motorista;
                set
                {
                    motorista = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TipoDocumento)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Documento)));
                }
            }

            public string UFEscolhida
            {
                get => Motorista.UF;
                set
                {
                    Motorista.UF = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
                }
            }

            public bool IsentoICMS
            {
                get => Motorista.InscricaoEstadual == "ISENTO";
                set
                {
                    Motorista.InscricaoEstadual = value ? "ISENTO" : null;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Motorista)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsentoICMS)));
                }
            }

            public int TipoDocumento { get; set; }
            public string Documento
            {
                get => Motorista.Documento; set
                {
                    var tipo = (TiposDocumento)TipoDocumento;
                    Motorista.CPF = tipo == TiposDocumento.CPF ? value : null;
                    Motorista.CNPJ = tipo == TiposDocumento.CNPJ ? value : null;
                }
            }

            public MotoristaDataContext(Motorista motorista)
            {
                Motorista = motorista;
                TipoDocumento = (int)motorista.TipoDocumento;
            }
        }
    }
}
