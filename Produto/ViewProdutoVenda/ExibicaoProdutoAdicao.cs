using BaseGeral.ItensBD;
using System.ComponentModel;
using Windows.UI.Xaml;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ViewProdutoVenda
{
    public sealed class ProdutoAdicao
    {
        public ProdutoDI Base { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public double Estoque { get; set; }
        public double Preco { get; set; }
    }

    public sealed class ExibicaoProdutoAdicao : INotifyPropertyChanged
    {
        public ProdutoDI Base { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Estoque => EstoqueDouble.ToString("N");
        public double EstoqueDouble { get; set; }
        public string Preco => PrecoDouble.ToString("C");
        public double PrecoDouble { get; set; }
        public Visibility Ativo => (Visibility)(EstoqueDouble > 0 ? 0 : 1);

        public event PropertyChangedEventHandler PropertyChanged;

        public static explicit operator ProdutoAdicao(ExibicaoProdutoAdicao exib)
        {
            return new ProdutoAdicao
            {
                Base = exib.Base,
                Codigo = exib.Codigo,
                Nome = exib.Nome,
                Estoque = exib.EstoqueDouble,
                Preco = exib.PrecoDouble
            };
        }

        public static implicit operator ExibicaoProdutoAdicao(ProdutoAdicao exib)
        {
            return new ExibicaoProdutoAdicao
            {
                Base = exib.Base,
                Codigo = exib.Codigo,
                Nome = exib.Nome,
                EstoqueDouble = exib.Estoque,
                PrecoDouble = exib.Preco
            };
        }

        public void AplicarAlteracoes()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ativo)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Estoque)));
        }
    }
}
