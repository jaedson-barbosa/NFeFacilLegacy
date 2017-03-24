using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace NFeFacil.ViewModel
{
    public sealed class DeclaracaoExportacaoDataContext : INotifyPropertyChanged
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
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Declaracao.ExportInd)));
            }
        }

        public Visibility VisibilidadeIndireta { get; private set; } = Visibility.Collapsed;

        public DeclaracaoExportacaoDataContext()
        {
            Declaracao = new GrupoExportacao();
        }
    }
}
