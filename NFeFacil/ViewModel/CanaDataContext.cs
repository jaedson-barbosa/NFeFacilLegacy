using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.View.CaixasDialogo;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace NFeFacil.ViewModel
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

        public ObservableCollection<FornecimentoDiário> ListaFornecimentoDiario
        {
            get { return Cana.forDia.GerarObs(); }
        }

        public ObservableCollection<Deduções> ListaDeducoes
        {
            get { return Cana.deduc.GerarObs(); }
        }

        public ICommand AdicionarFornecimentoCommand { get; }
        public ICommand RemoverFornecimentoCommand { get; }
        public ICommand AdicionarDeducaoCommand { get; }
        public ICommand RemoverDeducaoCommand { get; }

        public async void AdicionarFornecimento()
        {
            var caixa = new FornecimentoDiario();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                Cana.forDia.Add(x.DataContext as FornecimentoDiário);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListaFornecimentoDiario)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverFornecimento(FornecimentoDiário fornecimento)
        {
            Cana.forDia.Remove(fornecimento);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListaFornecimentoDiario)));
        }

        public async void AdicionarDeducao()
        {
            var caixa = new Deducao();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                Cana.deduc.Add(x.DataContext as Deduções);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListaDeducoes)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverDeducao(Deduções deducao)
        {
            Cana.deduc.Remove(deducao);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListaDeducoes)));
        }

        public CanaDataContext()
        {
            AdicionarFornecimentoCommand = new ComandoSemParametros(AdicionarFornecimento, true);
            RemoverFornecimentoCommand = new ComandoComParametros<FornecimentoDiário>(RemoverFornecimento, new ObterDataContext<FornecimentoDiário>());
            AdicionarDeducaoCommand = new ComandoSemParametros(AdicionarDeducao, true);
            RemoverDeducaoCommand = new ComandoComParametros<Deduções>(RemoverDeducao, new ObterDataContext<Deduções>());
        }
        public CanaDataContext(ref RegistroAquisicaoCana registro) : this()
        {
            Cana = registro;
        }
    }
}
