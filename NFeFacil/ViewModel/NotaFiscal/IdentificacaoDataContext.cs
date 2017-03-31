using NFeFacil.IBGE;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public class IdentificacaoDataContext : INotifyPropertyChanged
    {
        public Identificacao Ident { get; }

        public IdentificacaoDataContext() : base() { }
        public IdentificacaoDataContext(ref Identificacao ident)
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
                if (_DataEmissao == default(DateTime)) _DataEmissao = Convert.ToDateTime(Ident.DataHoraEmissão);
                return _DataEmissao;
            }
            set
            {
                _DataEmissao = new DateTime(value.Year, value.Month, value.Day, _DataEmissao.Hour, _DataEmissao.Minute, _DataEmissao.Second);
                Ident.DataHoraEmissão = _DataEmissao.ToStringPersonalizado();
            }
        }
        [XmlIgnore]
        public TimeSpan HoraEmissao
        {
            get
            {
                if (_DataEmissao == default(DateTime)) _DataEmissao = Convert.ToDateTime(Ident.DataHoraEmissão);
                return  _DataEmissao.TimeOfDay;
            }
            set
            {
                _DataEmissao = new DateTime(_DataEmissao.Year, _DataEmissao.Month, _DataEmissao.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.DataHoraEmissão = _DataEmissao.ToStringPersonalizado();
            }
        }

        private DateTime _DataSaidaEntrada = default(DateTime);
        [XmlIgnore]
        public DateTimeOffset DataSaidaEntrada
        {
            get
            {
                if (_DataSaidaEntrada == default(DateTime)) _DataSaidaEntrada = Convert.ToDateTime(Ident.DataHoraSaídaEntrada);
                return _DataSaidaEntrada;
            }
            set
            {
                _DataSaidaEntrada = new DateTime(value.Year, value.Month, value.Day, _DataSaidaEntrada.Hour, _DataSaidaEntrada.Minute, _DataSaidaEntrada.Second);
                Ident.DataHoraSaídaEntrada = _DataSaidaEntrada.ToStringPersonalizado();
            }
        }
        [XmlIgnore]
        public TimeSpan HoraSaidaEntrada
        {
            get
            {
                if (_DataSaidaEntrada == default(DateTime)) _DataSaidaEntrada = Convert.ToDateTime(Ident.DataHoraSaídaEntrada);
                return _DataSaidaEntrada.TimeOfDay;
            }
            set
            {
                _DataSaidaEntrada = new DateTime(_DataSaidaEntrada.Year, _DataSaidaEntrada.Month, _DataSaidaEntrada.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.DataHoraSaídaEntrada = _DataSaidaEntrada.ToStringPersonalizado();
            }
        }
        [XmlIgnore]
        public int DestinoOperação
        {
            get
            {
                return Ident.IdentificadorDestino - 1;
            }
            set
            {
                Ident.IdentificadorDestino = (ushort)(value + 1);
            }
        }
        [XmlIgnore]
        public int TipoDanfe
        {
            get
            {
                return Ident.TipoImpressão - 1;
            }
            set
            {
                Ident.TipoImpressão = (ushort)(value + 1);
            }
        }
        [XmlIgnore]
        public int FinNFe
        {
            get
            {
                return Ident.FinalidadeEmissão - 1;
            }
            set
            {
                Ident.FinalidadeEmissão = (ushort)(value + 1);
            }
        }

        [XmlIgnore]
        public string UFEscolhida
        {
            get
            {
                var nulo = Ident.CódigoUF == default(ushort);
                return !nulo ? Estados.EstadosCache.First(x => x.Codigo == Ident.CódigoUF).Nome : null;
            }
            set
            {
                Ident.CódigoUF = Estados.EstadosCache.First(x => x.Nome == value).Codigo;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Municipios)));
            }
        }

        private IEnumerable<Municipio> _Municipios
        {
            get
            {
                if (UFEscolhida != null)
                {
                    return IBGE.Municipios.Get(UFEscolhida);
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
                if (Ident.CodigoMunicípio != default(long))
                    return _Municipios.FirstOrDefault(x => x.CodigoMunicípio == Ident.CodigoMunicípio).Nome;
                else if (Municipios.Count() != 0)
                    return Municipios.First();
                else
                    return "";
            }
            set
            {
                Ident.CodigoMunicípio = _Municipios.First(x => x.Nome == value).CodigoMunicípio;
            }
        }
    }
}
