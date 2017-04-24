using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace NFeFacil.View.CaixasDialogo
{
    public sealed class ConverterContextoDeclaracaoExportacao : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is GrupoExportacao grupo)
            {
                return new DeclaracaoExportacaoDataContext(grupo);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DeclaracaoExportacaoDataContext contexto)
            {
                return contexto.Declaracao;
            }
            throw new ArgumentException();
        }

        private sealed class DeclaracaoExportacaoDataContext : INotifyPropertyChanged
        {
            public GrupoExportacao Declaracao { get; }

            public event PropertyChangedEventHandler PropertyChanged;

            private bool exportIndireta;
            public bool ExportIndireta
            {
                get { return exportIndireta; }
                set
                {
                    exportIndireta = value;
                    if (value)
                    {
                        VisibilidadeIndireta = Visibility.Visible;
                        Declaracao.ExportInd = new ExportacaoIndireta();
                    }
                    else
                    {
                        VisibilidadeIndireta = Visibility.Collapsed;
                        Declaracao.ExportInd = null;
                    }
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(VisibilidadeIndireta)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Declaracao)));
                }
            }

            public Visibility VisibilidadeIndireta { get; private set; } = Visibility.Collapsed;

            public DeclaracaoExportacaoDataContext(GrupoExportacao grupo)
            {
                Declaracao = grupo;
            }
        }
    }
}
