using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ViewModel
{
    public class IdentificaçãoDataContext : INotifyPropertyChanged
    {
        public Identificacao Ident { get; }

        public IdentificaçãoDataContext() : base() { }
        public IdentificaçãoDataContext(ref Identificacao ident)
        {
            Ident = ident;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _DataEmissao = default(DateTime);
        [XmlIgnore]
        public DateTimeOffset DataEmissao
        {
            get
            {
                if (_DataEmissao == default(DateTime)) _DataEmissao = Ident.dataHoraEmissão.ToDateTime();
                return _DataEmissao;
            }
            set
            {
                _DataEmissao = new DateTime(value.Year, value.Month, value.Day, _DataEmissao.Hour, _DataEmissao.Minute, _DataEmissao.Second);
                Ident.dataHoraEmissão = _DataEmissao.ToStringPersonalizado();
            }
        }
        [XmlIgnore]
        public TimeSpan HoraEmissao
        {
            get
            {
                if (_DataEmissao == default(DateTime)) _DataEmissao = Ident.dataHoraEmissão.ToDateTime();
                return  _DataEmissao.TimeOfDay;
            }
            set
            {
                _DataEmissao = new DateTime(_DataEmissao.Year, _DataEmissao.Month, _DataEmissao.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.dataHoraEmissão = _DataEmissao.ToStringPersonalizado();
            }
        }

        private DateTime _DataSaidaEntrada = default(DateTime);
        [XmlIgnore]
        public DateTimeOffset DataSaidaEntrada
        {
            get
            {
                if (_DataSaidaEntrada == default(DateTime)) _DataSaidaEntrada = Ident.dataHoraSaídaEntrada.ToDateTime();
                return _DataSaidaEntrada;
            }
            set
            {
                _DataSaidaEntrada = new DateTime(value.Year, value.Month, value.Day, _DataSaidaEntrada.Hour, _DataSaidaEntrada.Minute, _DataSaidaEntrada.Second);
                Ident.dataHoraSaídaEntrada = _DataSaidaEntrada.ToStringPersonalizado();
            }
        }
        [XmlIgnore]
        public TimeSpan HoraSaidaEntrada
        {
            get
            {
                if (_DataSaidaEntrada == default(DateTime)) _DataSaidaEntrada = Ident.dataHoraSaídaEntrada.ToDateTime();
                return _DataSaidaEntrada.TimeOfDay;
            }
            set
            {
                _DataSaidaEntrada = new DateTime(_DataSaidaEntrada.Year, _DataSaidaEntrada.Month, _DataSaidaEntrada.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.dataHoraSaídaEntrada = _DataSaidaEntrada.ToStringPersonalizado();
            }
        }
        [XmlIgnore]
        public int DestinoOperação
        {
            get
            {
                return Ident.identificadorDestino - 1;
            }
            set
            {
                Ident.identificadorDestino = (ushort)(value + 1);
            }
        }
        [XmlIgnore]
        public int TipoDanfe
        {
            get
            {
                return Ident.tipoImpressão - 1;
            }
            set
            {
                Ident.tipoImpressão = (ushort)(value + 1);
            }
        }
        [XmlIgnore]
        public int FinNFe
        {
            get
            {
                return Ident.finalidadeEmissão - 1;
            }
            set
            {
                Ident.finalidadeEmissão = (ushort)(value + 1);
            }
        }

        private IEnumerable<Estado> _UFs = Estados.Buscar();
        [XmlIgnore]
        public IEnumerable<string> UFs
        {
            get { return from uf in _UFs select uf.Nome; }
        }
        [XmlIgnore]
        public string UFEscolhida
        {
            get
            {
                var nulo = Ident.códigoUF == default(ushort);
                return !nulo ? _UFs.First(x => x.Codigo == Ident.códigoUF).Nome : null;
            }
            set
            {
                Ident.códigoUF = _UFs.First(x => x.Nome == value).Codigo;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Municipios)));
            }
        }

        private IEnumerable<Municipio> _Municipios
        {
            get
            {
                if (UFEscolhida != null)
                {
                    var escolhido = _UFs.First(x => x.Nome == UFEscolhida);
                    return new ObservableCollection<Municipio>(Informacoes.IBGE.Municipios.Buscar(escolhido));
                }
                return new List<Municipio>();
            }
        }
        [XmlIgnore]
        public IEnumerable<string> Municipios
        {
            get { return from mon in _Municipios select mon.Nome; }
        }
        [XmlIgnore]
        public string MunicipioEscolhido
        {
            get
            {
                if (Ident.codigoMunicípio != default(long))
                    return _Municipios.FirstOrDefault(x => x.CodigoMunicípio == Ident.codigoMunicípio).Nome;
                else if (Municipios.Count() != 0)
                    return Municipios.First();
                else
                    return "";
            }
            set
            {
                Ident.codigoMunicípio = _Municipios.First(x => x.Nome == value).CodigoMunicípio;
            }
        }
    }
}
