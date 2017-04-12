using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using System.ComponentModel;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public class IdentificacaoDataContext
    {
        public Identificacao Ident { get; }

        public IdentificacaoDataContext() : base() { }
        public IdentificacaoDataContext(ref Identificacao ident)
        {
            Ident = ident;
        }

        public DateTimeOffset DataEmissao
        {
            get
            {
                if (string.IsNullOrEmpty(Ident.DataHoraEmissão))
                {
                    var agora = DateTime.Now;
                    Ident.DataHoraEmissão = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(Ident.DataHoraEmissão);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraEmissão);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                Ident.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraEmissao
        {
            get => DataEmissao.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraEmissão);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public DateTimeOffset DataSaidaEntrada
        {
            get
            {
                if (string.IsNullOrEmpty(Ident.DataHoraSaídaEntrada))
                {
                    var agora = DateTime.Now;
                    Ident.DataHoraSaídaEntrada = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(Ident.DataHoraSaídaEntrada);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraSaídaEntrada);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                Ident.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraSaidaEntrada
        {
            get => DataSaidaEntrada.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraSaídaEntrada);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }
    }
}
