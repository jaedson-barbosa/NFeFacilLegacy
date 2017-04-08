using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.View.CaixasDialogo;
using System;
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
            get => Cobranca.Fat ?? (Cobranca.Fat = new Fatura());
            set => Cobranca.Fat = value;
        }

        public CobrancaDataContext(ref Cobranca cobranca)
        {
            Cobranca = cobranca;
            AdicionarDuplicataCommand = new ComandoSemParametros(AdicionarDuplicata, true);
            RemoverDuplicataCommand = new ComandoComParametros<Duplicata, ObterDataContext<Duplicata>>(RemoverDuplicata);
        }

        public ICommand AdicionarDuplicataCommand { get; }
        public ICommand RemoverDuplicataCommand { get; }

        private async void AdicionarDuplicata()
        {
            var caixa = new AdicionarDuplicata();
            caixa.PrimaryButtonClick += (sender, e) =>
              {
                  Cobranca.Dup.Add(sender.DataContext as Duplicata);
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cobranca"));
              };
            await caixa.ShowAsync();
        }

        private void RemoverDuplicata(Duplicata duplicata)
        {
            Cobranca.Dup.Remove(duplicata);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cobranca"));
        }
    }
}
