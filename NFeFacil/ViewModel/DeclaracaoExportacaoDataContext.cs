using System.ComponentModel;
using Windows.UI.Xaml;

namespace NFeFacil.ViewModel
{
    public sealed class DeclaracaoExportacaoDataContext : INotifyPropertyChanged
    {
        public GrupoExportação Declaracao { get; }

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
                    Declaracao.exportInd = new ExportaçãoIndireta();
                }
                else
                {
                    VisibilidadeIndireta = Visibility.Collapsed;
                    Declaracao.exportInd = null;
                }
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(VisibilidadeIndireta)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Declaracao.exportInd)));
            }
        }

        public Visibility VisibilidadeIndireta { get; private set; } = Visibility.Collapsed;

        public DeclaracaoExportacaoDataContext()
        {
            Declaracao = new GrupoExportação();
        }
    }
}
