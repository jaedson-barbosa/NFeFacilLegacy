using BibliotecaCentral.ItensBD;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.CaixasDialogo.RegistroVenda
{
    public sealed partial class CalculoDesconto : ContentDialog
    {
        public List<ProdutoSimplesVenda> Produtos { get; }

        double porcentagem;
        double Porcentagem
        {
            get => porcentagem;
            set
            {
                porcentagem = value;
                CalcularPelaPorcentagem(value);
            }
        }

        double valorDesejado;
        double ValorDesejado
        {
            get => valorDesejado;
            set
            {
                porcentagem = value;
                CalcularPeloValorDesejado(value);
            }
        }

        public CalculoDesconto(List<ProdutoSimplesVenda> produtos)
        {
            this.InitializeComponent();
            Produtos = produtos;
        }

        void CalcularPelaPorcentagem(double porcentagem)
        {
            for (int i = 0; i < Produtos.Count; i++)
            {
                var atual = Produtos[i];
                var valorOriginal = atual.ValorUnitario * atual.Quantidade;
                var porcentagemUsada = (100 - porcentagem) / 100;
                var desconto = valorOriginal * porcentagemUsada;
                atual.Desconto = desconto;
                atual.CalcularTotalLíquido();
            }
            ValorDesejado = Produtos.Sum(x => x.Quantidade * x.ValorUnitario);
        }

        void CalcularPeloValorDesejado(double valor)
        {
            var totalOriginal = Produtos.Sum(x => x.Quantidade * x.ValorUnitario);
            var porcentagemDesejada = valor / totalOriginal;
            for (int i = 0; i < Produtos.Count; i++)
            {
                var atual = Produtos[i];
                var valorOriginal = atual.ValorUnitario * atual.Quantidade;
                var desconto = valorOriginal * porcentagemDesejada;
                atual.Desconto = desconto;
                atual.CalcularTotalLíquido();
            }
            Porcentagem = 100 - (porcentagemDesejada * 100);
        }
    }
}
