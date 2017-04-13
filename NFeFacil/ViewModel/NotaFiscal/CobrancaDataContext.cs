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

        public CobrancaDataContext(ref Cobranca cobranca)
        {
            Cobranca = cobranca;
        }

        public ICommand AdicionarDuplicataCommand => new ComandoSemParametros(AdicionarDuplicata, true);
        public ICommand RemoverDuplicataCommand => new ComandoComParametros<Duplicata, ObterDataContext<Duplicata>>(RemoverDuplicata);

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
