using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class CobrancaDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Cobranca Cobranca { get; }

        public Fatura Fat
        {
            get
            {
                if (Cobranca.Fat == null)
                {
                    Cobranca.Fat = new Fatura();
                }
                return Cobranca.Fat;
            }
            set
            {
                Cobranca.Fat = value;
            }
        }

        public DuplicataDataContext NovaDuplicata { get; set; }
        public ObservableCollection<Duplicata> Duplicatas
        {
            get { return Cobranca.Dup.GerarObs(); }
        }

        public int IndexDuplicataEscolhida { get; set; }

        public CobrancaDataContext(ref Cobranca cobranca)
        {
            Cobranca = cobranca;
            NovaDuplicata = new DuplicataDataContext();
            AdicionarDuplicataCommand = new ComandoSemParametros(AdicionarDuplicata, true);
            RemoverDuplicataCommand = new ComandoSemParametros(RemoverDuplicata, true);
        }

        public ICommand AdicionarDuplicataCommand { get; }
        public ICommand RemoverDuplicataCommand { get; }

        private void AdicionarDuplicata()
        {
            Cobranca.Dup.Add(NovaDuplicata._Duplicata);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Duplicatas)));
            NovaDuplicata = new DuplicataDataContext();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NovaDuplicata)));
        }

        private void RemoverDuplicata()
        {
            if (IndexDuplicataEscolhida != -1 && Cobranca.Dup.Count > 0)
            {
                Cobranca.Dup.RemoveAt(IndexDuplicataEscolhida);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Duplicatas)));
            }
        }
    }
}
