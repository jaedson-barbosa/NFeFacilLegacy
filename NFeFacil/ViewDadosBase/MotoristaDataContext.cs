using System.ComponentModel;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ViewModel;

namespace NFeFacil.ViewDadosBase
{
    public sealed class MotoristaDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MotoristaDI Motorista { get; set; }

        public ObservableCollection<VeiculoDI> Veiculos { get; }
        public ICommand AdicionarVeiculoCommand { get; }
        public ICommand RemoverVeiculoCommand { get; }

        public string UFEscolhida
        {
            get => Motorista.UF;
            set
            {
                Motorista.UF = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
            }
        }

        public bool IsentoICMS
        {
            get => Motorista.InscricaoEstadual == "ISENTO";
            set
            {
                Motorista.InscricaoEstadual = value ? "ISENTO" : null;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Motorista)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsentoICMS)));
            }
        }

        public int TipoDocumento { get; set; }
        public string Documento
        {
            get => Motorista.Documento;
            set
            {
                var tipo = (TiposDocumento)TipoDocumento;
                Motorista.CPF = tipo == TiposDocumento.CPF ? value : null;
                Motorista.CNPJ = tipo == TiposDocumento.CNPJ ? value : null;
            }
        }

        public MotoristaDataContext(ref MotoristaDI motorista)
        {
            Motorista = motorista;
            TipoDocumento = (int)motorista.TipoDocumento;
            using (var db = new AplicativoContext())
            {
                Veiculos = new ObservableCollection<VeiculoDI>(db.Veiculos);
            }
            AdicionarVeiculoCommand = new Comando(AdicionarVeiculo);
            RemoverVeiculoCommand = new Comando<VeiculoDI>(RemoverVeiculo);
        }

        async void AdicionarVeiculo()
        {
            var caixa = new View.CaixasDialogo.AdicionarVeiculo();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var veic = (VeiculoDI)caixa.DataContext;
                using (var db = new AplicativoContext())
                {
                    db.Veiculos.Add(veic);
                    db.SaveChanges();
                    Veiculos.Add(veic);
                }
            }
        }

        void RemoverVeiculo(VeiculoDI veiculo)
        {
            using (var db = new AplicativoContext())
                db.Veiculos.Remove(veiculo);
            Veiculos.Remove(veiculo);
        }
    }
}
