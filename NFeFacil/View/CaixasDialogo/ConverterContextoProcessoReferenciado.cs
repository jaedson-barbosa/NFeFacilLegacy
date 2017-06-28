using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using Windows.UI.Xaml.Data;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.CaixasDialogo
{
    public sealed class ConverterContextoProcessoReferenciado : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ProcessoReferenciado proc)
            {
                return new ProcessoReferenciadoDataContext(ref proc);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is ProcessoReferenciadoDataContext proc)
            {
                return proc.Processo;
            }
            throw new ArgumentException();
        }

        public sealed class ProcessoReferenciadoDataContext
        {
            public ProcessoReferenciado Processo { get; }

            public int Origem
            {
                get => Processo.indProc == 9 ? 4 : Processo.indProc;
                set => Processo.indProc = value == 4 ? 9 : value;
            }

            public ProcessoReferenciadoDataContext(ref ProcessoReferenciado proc) => Processo = proc;
        }
    }
}
