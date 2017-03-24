using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public sealed class CobrancaDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Cobrança Cobranca { get; }

        public Fatura Fat
        {
            get
            {
                if (Cobranca.fat == null)
                {
                    Cobranca.fat = new Fatura();
                }
                return Cobranca.fat;
            }
            set
            {
                Cobranca.fat = value;
            }
        }

        public DuplicataDataContext NovaDuplicata { get; set; }
        public ObservableCollection<Duplicata> Duplicatas
        {
            get { return Cobranca.dup.GerarObs(); }
        }

        public int IndexDuplicataEscolhida { get; set; }

        public CobrancaDataContext(ref Cobrança cobranca)
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
            Cobranca.dup.Add(NovaDuplicata._Duplicata);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Duplicatas)));
            NovaDuplicata = new DuplicataDataContext();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NovaDuplicata)));
        }

        private void RemoverDuplicata()
        {
            if (IndexDuplicataEscolhida != -1 && Cobranca.dup.Count > 0)
            {
                Cobranca.dup.RemoveAt(IndexDuplicataEscolhida);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Duplicatas)));
            }
        }
    }
}
