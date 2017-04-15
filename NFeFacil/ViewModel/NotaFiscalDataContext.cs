using NFeFacil.IBGE;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NFeFacil.ViewModel
{
    public sealed class NotaFiscalDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        #region Identificação

        public Identificacao Ident { get; }

        public DateTimeOffset DataEmissao
        {
            get
            {
                if (string.IsNullOrEmpty(Ident.DataHoraEmissão))
                {
                    var agora = DateTime.Now;
                    Ident.DataHoraEmissão = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(Ident.DataHoraEmissão);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraEmissão);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                Ident.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraEmissao
        {
            get => DataEmissao.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraEmissão);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public DateTimeOffset DataSaidaEntrada
        {
            get
            {
                if (string.IsNullOrEmpty(Ident.DataHoraSaídaEntrada))
                {
                    var agora = DateTime.Now;
                    Ident.DataHoraSaídaEntrada = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(Ident.DataHoraSaídaEntrada);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraSaídaEntrada);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                Ident.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraSaidaEntrada
        {
            get => DataSaidaEntrada.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraSaídaEntrada);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        #endregion

        #region Transporte

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

        public ICommand AdicionarReboqueCommand => new ComandoSemParametros(AdicionarReboque, true);
        public ICommand RemoverReboqueCommand => new ComandoComParametros<Reboque, ObterDataContext<Reboque>>(RemoverReboque);
        public ICommand AdicionarVolumeCommand => new ComandoSemParametros(AdicionarVolume, true);
        public ICommand RemoverVolumeCommand => new ComandoComParametros<Volume, ObterDataContext<Volume>>(RemoverVolume);

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
            add.PrimaryButtonClick += (x, y) =>
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

        #endregion

        #region Cobrança

        public Cobranca Cobranca { get; }

        public ICommand AdicionarDuplicataCommand => new ComandoSemParametros(AdicionarDuplicata, true);
        public ICommand RemoverDuplicataCommand => new ComandoComParametros<Duplicata, ObterDataContext<Duplicata>>(RemoverDuplicata);

        private async void AdicionarDuplicata()
        {
            var caixa = new View.CaixasDialogo.AdicionarDuplicata();
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

        #endregion;

        #region Cana

        public RegistroAquisicaoCana Cana { get; }

        public ICommand AdicionarFornecimentoCommand => new ComandoSemParametros(AdicionarFornecimento, true);
        public ICommand RemoverFornecimentoCommand => new ComandoComParametros<FornecimentoDiario, ObterDataContext<FornecimentoDiario>>(RemoverFornecimento);
        public ICommand AdicionarDeducaoCommand => new ComandoSemParametros(AdicionarDeducao, true);
        public ICommand RemoverDeducaoCommand => new ComandoComParametros<Deducoes, ObterDataContext<Deducoes>>(RemoverDeducao);

        public async void AdicionarFornecimento()
        {
            var caixa = new View.CaixasDialogo.AdicionarFornecimentoDiario();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                Cana.forDia.Add(x.DataContext as FornecimentoDiario);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverFornecimento(FornecimentoDiario fornecimento)
        {
            Cana.forDia.Remove(fornecimento);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
        }

        public async void AdicionarDeducao()
        {
            var caixa = new View.CaixasDialogo.AdicionarDeducao();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                Cana.deduc.Add(x.DataContext as Deducoes);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverDeducao(Deducoes deducao)
        {
            Cana.deduc.Remove(deducao);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
        }

        #endregion
    }
}
