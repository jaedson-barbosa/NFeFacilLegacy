using NFeFacil.IBGE;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class MotoristaDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Motorista Motorista { get; }

        public ObservableCollection<TiposDocumento> Tipos { get; } = Enum.GetValues(typeof(TiposDocumento)).Cast<TiposDocumento>().GerarObs();

        [XmlIgnore]
        private IEnumerable<Estado> _UFs
        {
            get
            {
                var estados = Estados.Buscar().ToList();
                var estado = new Estado
                {
                    Nome = "Exterior",
                    Sigla = "EX"
                };
                estados.Add(estado);
                return estados;
            }
        }

        [XmlIgnore]
        public ObservableCollection<string> Municipios
        {
            get
            {
                if (string.IsNullOrEmpty(Motorista.UF))
                    return new ObservableCollection<string>();
                else
                    return (from mun in IBGE.Municipios.Buscar(_UFs.First(x => x.Sigla == Motorista.UF))
                            select mun.Nome).GerarObs();
            }
        }

        [XmlIgnore]
        public string UFEscolhida
        {
            get
            {
                return Motorista.UF;
            }
            set
            {
                Motorista.UF = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Municipios)));
            }
        }

        public int TipoDocumento { get; set; }
        public string Documento
        {
            get { return Motorista.obterDocumento; }
            set
            {
                switch ((TiposDocumento)TipoDocumento)
                {
                    case TiposDocumento.CPF:
                        Motorista.CPF = value;
                        Motorista.CNPJ = null;
                        break;
                    case TiposDocumento.CNPJ:
                        Motorista.CPF = null;
                        Motorista.CNPJ = value;
                        break;
                    case TiposDocumento.idEstrangeiro:
                        new Exception("Não existe motorista extrangeiro.");
                        break;
                }
            }
        }

        public MotoristaDataContext() : base() { }
        public MotoristaDataContext(ref Motorista motorista)
        {
            Motorista = motorista;
        }
    }
}
