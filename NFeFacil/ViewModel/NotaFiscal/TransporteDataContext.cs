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

        public MotoristaDataContext Motorista
        {
            get
            {
                if (Transp.transporta == null)
                    Transp.transporta = new Motorista();
                return new MotoristaDataContext(ref Transp.transporta);
            }
            set
            {
                Transp.transporta = value.Motorista;
            }
        }

        public Veiculo VeicTransp
        {
            get
            {
                if (Transp.veicTransp == null)
                    Transp.veicTransp = new Veiculo();
                return Transp.veicTransp;
            }
            set
            {
                Transp.veicTransp = value;
            }
        }

        public ICMSTransporteDataContext RetTransp
        {
            get
            {
                if (Transp.retTransp == null)
                    Transp.retTransp = new ICMSTransporte();
                return new ICMSTransporteDataContext(ref Transp.retTransp);
            }
            set
            {
                Transp.retTransp = value.ICMS;
            }
        }

        public ObservableCollection<ModalidadesTransporte> Modalidades
        {
            get
            {
                return Enum.GetValues(typeof(ModalidadesTransporte)).Cast<ModalidadesTransporte>().GerarObs();
            }
        }

        public ModalidadesTransporte ModFrete
        {
            get { return (ModalidadesTransporte)Transp.modFrete; }
            set { Transp.modFrete = (int)value; }
        }

        public ObservableCollection<Reboque> Reboques
        {
            get { return Transp.reboque.GerarObs(); }
        }
        public Reboque NovoReboque { get; private set; }
        public int IndexReboqueSelecionado { get; set; }

        public ObservableCollection<Volume> Volumes
        {
            get { return Transp.vol.GerarObs(); }
        }
        public Volume VolumeSelecionado { get; set; }

        public TransporteDataContext() : base() { }
        public TransporteDataContext(ref Transporte transp)
        {
            Transp = transp;
            NovoReboque = new Reboque();
            AdicionarReboqueCommand = new ComandoSemParametros(AdicionarReboque, true);
            RemoverReboqueCommand = new ComandoSemParametros(RemoverReboque, true);
            AdicionarVolumeCommand = new ComandoSemParametros(AdicionarVolume, true);
            RemoverVolumeCommand = new ComandoSemParametros(RemoverVolume, true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Transporte ObterTranporteNormalizado()
        {
            var transp = Transp;
            transp.transporta = ÉDefault(Transp.transporta) ? new Motorista(Transp.transporta) : null;
            transp.retTransp = ÉDefault(Transp.retTransp) ? new ICMSTransporte(ref Transp.retTransp) : null;
            return transp;
        }

        private static bool ÉDefault<T>(T valor) where T : class => valor != default(T) && valor != null;

        public ICommand AdicionarReboqueCommand { get; }
        public ICommand RemoverReboqueCommand { get; }
        public ICommand AdicionarVolumeCommand { get; }
        public ICommand RemoverVolumeCommand { get; }

        private void AdicionarReboque()
        {
            Transp.reboque.Add(NovoReboque);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Reboques)));
            NovoReboque = new Reboque();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NovoReboque)));
        }

        private void RemoverReboque()
        {
            if (IndexReboqueSelecionado != -1 && Transp.reboque.Count > 0)
            {
                Transp.reboque.RemoveAt(IndexReboqueSelecionado);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Reboques)));
            }
        }

        private async void AdicionarVolume()
        {
            var add = new View.CaixasDialogo.AdicionarVolume();
            add.PrimaryButtonClick += (x,y)=>
            {
                var vol = x.DataContext as Volume;
                Transp.vol.Add(vol);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Volumes)));
            };
            await add.ShowAsync();
        }

        private void RemoverVolume()
        {
            if (VolumeSelecionado != null && Transp.vol.Count > 0)
            {
                Transp.vol.Remove(VolumeSelecionado);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Volumes)));
            }
        }
    }
}
