using NFeFacil.IBGE;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class TransporteDataContext : INotifyPropertyChanged
    {
        public Transporte Transp { get; }

        private Estado ufEscolhida;
        public Estado UFEscolhida
        {
            get
            {
                if (!string.IsNullOrEmpty(Transp.retTransp.cMunFG) && ufEscolhida == null)
                {
                    foreach (var item in Estados.EstadosCache)
                    {
                        var lista = Municipios.Get(item);
                        if (lista.Count(x => x.Codigo == int.Parse(Transp.retTransp.cMunFG)) > 0)
                        {
                            ufEscolhida = item;
                        }
                    }
                }
                return ufEscolhida;
            }
            set
            {
                ufEscolhida = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UFEscolhida"));
            }
        }

        public ObservableCollection<ModalidadesTransporte> Modalidades => Extensoes.ObterItens<ModalidadesTransporte>();

        public ModalidadesTransporte ModFrete
        {
            get => (ModalidadesTransporte)Transp.modFrete;
            set => Transp.modFrete = (int)value;
        }

        public TransporteDataContext() : base() { }
        public TransporteDataContext(ref Transporte transp)
        {
            Transp = transp;
            AdicionarReboqueCommand = new ComandoSemParametros(AdicionarReboque, true);
            RemoverReboqueCommand = new ComandoComParametros<Reboque, ObterDataContext<Reboque>>(RemoverReboque);
            AdicionarVolumeCommand = new ComandoSemParametros(AdicionarVolume, true);
            RemoverVolumeCommand = new ComandoComParametros<Volume, ObterDataContext<Volume>>(RemoverVolume);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Transporte ObterTranporteNormalizado()
        {
            var transp = Transp;
            transp.transporta = NaoEDefault(Transp.transporta) ? Transp.transporta : null;
            transp.veicTransp = NaoEDefault(Transp.veicTransp) ? Transp.veicTransp : null;
            transp.retTransp = NaoEDefault(Transp.retTransp) ? Transp.retTransp : null;
            return transp;
        }

        private static bool NaoEDefault<T>(T valor) where T : class => valor != default(T) && valor != null;

        public ICommand AdicionarReboqueCommand { get; }
        public ICommand RemoverReboqueCommand { get; }
        public ICommand AdicionarVolumeCommand { get; }
        public ICommand RemoverVolumeCommand { get; }

        private async void AdicionarReboque()
        {
            var add = new View.CaixasDialogo.AdicionarReboque();
            add.PrimaryButtonClick += (x, y) =>
            {
                Transp.reboque.Add(x.DataContext as Reboque);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transp)));
            };
            await add.ShowAsync();
        }

        private void RemoverReboque(Reboque reboque)
        {
            Transp.reboque.Remove(reboque);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transp)));
        }

        private async void AdicionarVolume()
        {
            var add = new View.CaixasDialogo.AdicionarVolume();
            add.PrimaryButtonClick += (x,y)=>
            {
                Transp.vol.Add(x.DataContext as Volume);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transp)));
            };
            await add.ShowAsync();
        }

        private void RemoverVolume(Volume volume)
        {
            Transp.vol.Remove(volume);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transp)));
        }
    }
}
