using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace RegistroComum.RelatorioProduto01
{
    public sealed partial class TabelaSimples : UserControl
    {
        internal ObservableCollection<ExibicaoProduto> Produtos { get; set; }
        ObservableCollection<string> Codigos, Nomes, Precos, Estoques;
        Visibility LinhaTitulo;

        internal TabelaSimples(bool contemLinhaTitulo)
        {
            InitializeComponent();
            LinhaTitulo = contemLinhaTitulo ? Visibility.Visible : Visibility.Collapsed;
            Produtos = new ObservableCollection<ExibicaoProduto>();
            Codigos = new ObservableCollection<string>();
            Nomes = new ObservableCollection<string>();
            Precos = new ObservableCollection<string>();
            Estoques = new ObservableCollection<string>();
            Produtos.CollectionChanged += Produtos_CollectionChanged;
        }

        private void Produtos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    var atual = (ExibicaoProduto)e.NewItems[i];
                    Codigos.Add(atual.Codigo);
                    Nomes.Add(atual.Nome);
                    Precos.Add(atual.Preco);
                    Estoques.Add(atual.Estoque);
                }
            }
            else
                throw new System.NotImplementedException();
        }
    }
}
