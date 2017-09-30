using NFeFacil.ItensBD;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class CalculoDesconto : ContentDialog
    {
        public List<ProdutoSimplesVenda> Produtos { get; }

        public CalculoDesconto(List<ProdutoSimplesVenda> produtos)
        {
            InitializeComponent();
            Produtos = produtos;
            var valorDesejado = produtos.Sum(x => x.Quantidade * x.ValorUnitario);
            txtValorDesejado.Number = valorDesejado;
        }

        void CalcularPelaPorcentagem(double porcentagem)
        {
            for (int i = 0; i < Produtos.Count; i++)
            {
                var atual = Produtos[i];
                var valorOriginal = atual.ValorUnitario * atual.Quantidade;
                var porcentagemUsada = porcentagem / 100;
                var desconto = valorOriginal * porcentagemUsada;
                atual.Desconto = desconto;
                atual.CalcularTotalLíquido();
            }
            var valorDesejado = Produtos.Sum(x => x.Quantidade * x.ValorUnitario - x.Desconto);
            txtValorDesejado.Number = valorDesejado;
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
            var porcentagem = 100 - (porcentagemDesejada * 100);
            sldDesconto.Value = porcentagem;
        }

        private void ValorDesejadoChanged(View.Controles.EntradaNumerica sender, View.Controles.NumeroChangedEventArgs e)
        {
            if (Produtos != null)
            {
                CalcularPeloValorDesejado(e.NovoNumero);
            }
        }

        private void sldDesconto_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Produtos != null)
            {
                CalcularPelaPorcentagem(e.NewValue);
            }
        }
    }
}
