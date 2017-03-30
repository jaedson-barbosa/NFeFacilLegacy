using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.View.CaixasDialogo;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class CanaDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public RegistroAquisicaoCana Cana { get; }

        public DateTimeOffset MesAnoReferencia
        {
            get
            {
                if (Cana.referencia == null) return DateTimeOffset.Now;
                else return DateTimeOffset.ParseExact(Cana.referencia, "MM/yyyy", CultureInfo.InvariantCulture);
            }
            set { Cana.referencia = value.ToString("MM/yyyy"); }
        }

        public ObservableCollection<ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.FornecimentoDiario> ListaFornecimentoDiario
        {
            get { return Cana.forDia.GerarObs(); }
        }

        public ObservableCollection<Deducoes> ListaDeducoes
        {
            get { return Cana.deduc.GerarObs(); }
        }

        public ICommand AdicionarFornecimentoCommand { get; }
        public ICommand RemoverFornecimentoCommand { get; }
        public ICommand AdicionarDeducaoCommand { get; }
        public ICommand RemoverDeducaoCommand { get; }

        public async void AdicionarFornecimento()
        {
            var caixa = new View.CaixasDialogo.FornecimentoDiario();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                Cana.forDia.Add(x.DataContext as ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.FornecimentoDiario);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListaFornecimentoDiario)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverFornecimento(ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.FornecimentoDiario fornecimento)
        {
            Cana.forDia.Remove(fornecimento);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListaFornecimentoDiario)));
        }

        public async void AdicionarDeducao()
        {
            var caixa = new Deducao();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                Cana.deduc.Add(x.DataContext as Deducoes);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListaDeducoes)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverDeducao(Deducoes deducao)
        {
            Cana.deduc.Remove(deducao);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListaDeducoes)));
        }

        public CanaDataContext()
        {
            AdicionarFornecimentoCommand = new ComandoSemParametros(AdicionarFornecimento, true);
            RemoverFornecimentoCommand = new ComandoComParametros<ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.FornecimentoDiario, ObterDataContext<ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.FornecimentoDiario>>(this.RemoverFornecimento);
            AdicionarDeducaoCommand = new ComandoSemParametros(AdicionarDeducao, true);
            RemoverDeducaoCommand = new ComandoComParametros<Deducoes, ObterDataContext<Deducoes>>(RemoverDeducao);
        }
        public CanaDataContext(ref RegistroAquisicaoCana registro) : this()
        {
            Cana = registro;
        }
    }
}
