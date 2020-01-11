using System.ComponentModel;
using Windows.UI.Xaml;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace BaseGeral.View
{
    public sealed class EtapaProcesso : INotifyPropertyChanged
    {
        public EtapaProcesso(string descricao)
        {
            Descricao = descricao;
            Atual = Status.Pendente;
        }

        public Visibility Concluido => Atual == Status.Concluido ? Visibility.Visible : Visibility.Collapsed;
        public Visibility Pendente => Atual == Status.Pendente ? Visibility.Visible : Visibility.Collapsed;
        public bool EmAndamento => Atual == Status.EmAndamento;
        public string Descricao { get; set; }

        public Status Atual { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Update()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Concluido)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pendente)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmAndamento)));
        }

        public enum Status { Pendente, EmAndamento, Concluido }
    }
}
