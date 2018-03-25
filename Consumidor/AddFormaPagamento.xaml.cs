using BaseGeral.ModeloXML;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Consumidor
{
    public sealed partial class AddFormaPagamento : ContentDialog, INotifyPropertyChanged
    {
        public Pagamento Pagamento { get; } = new Pagamento();
        public event PropertyChangedEventHandler PropertyChanged;

        bool usarCartao;
        bool UsarCartao
        {
            get => usarCartao;
            set
            {
                usarCartao = value;
                stkCartao.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                Pagamento.Cartao = value ? new Cartao() : null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pagamento)));
            }
        }

        public AddFormaPagamento()
        {
            InitializeComponent();
        }
    }
}
