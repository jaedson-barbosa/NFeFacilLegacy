using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.View.CaixasDialogo;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class CanaDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public RegistroAquisicaoCana Cana { get; }

        public ICommand AdicionarFornecimentoCommand { get; }
        public ICommand RemoverFornecimentoCommand { get; }
        public ICommand AdicionarDeducaoCommand { get; }
        public ICommand RemoverDeducaoCommand { get; }

        public async void AdicionarFornecimento()
        {
            var caixa = new AdicionarFornecimentoDiario();
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
            var caixa = new AdicionarDeducao();
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

        public CanaDataContext()
        {
            AdicionarFornecimentoCommand = new ComandoSemParametros(AdicionarFornecimento, true);
            RemoverFornecimentoCommand = new ComandoComParametros<FornecimentoDiario, ObterDataContext<FornecimentoDiario>>(RemoverFornecimento);
            AdicionarDeducaoCommand = new ComandoSemParametros(AdicionarDeducao, true);
            RemoverDeducaoCommand = new ComandoComParametros<Deducoes, ObterDataContext<Deducoes>>(RemoverDeducao);
        }
        public CanaDataContext(ref RegistroAquisicaoCana registro) : this()
        {
            Cana = registro;
        }
    }
}
