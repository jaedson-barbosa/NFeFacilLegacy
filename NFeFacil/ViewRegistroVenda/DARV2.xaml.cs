using System;
using NFeFacil.ItensBD;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static NFeFacil.ExtensoesPrincipal;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DARV2 : Page
    {
        #region Dimensoes

        internal GridLength Largura2 { get; } = CentimeterToLength(2);
        internal GridLength Largura3 { get; } = CentimeterToLength(3);


        #endregion

        internal RegistroVenda Registro { get; }
        internal EmitenteDI Emitente { get; }
        internal ClienteDI Cliente { get; }
        internal Vendedor Vendedor { get; }
        internal Comprador Comprador { get; }
        internal MotoristaDI Motorista { get; }

        internal string Subtotal { get; }
        internal string Acrescimos { get; }
        internal string Desconto { get; }
        internal string Total { get; }

        internal string NomeAssinatura { get; }
        internal string Observacoes => Registro.Observações;

        public DARV2()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DadosImpressaoDARV venda = (DadosImpressaoDARV)e.Parameter;
            var dimensoes = venda.Dimensoes;
            var registro = venda.Venda;
            
        }

        struct ExibicaoProduto
        {
            public string CodigoProduto { get; }
            public string Descricao { get; }
            public string ValorUnitario { get; }
            public string TotalBruto { get; }
        }
    }

    sealed class MascaraDocumento : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            long numero;
            if (value is string str) numero = long.Parse(str.Trim());
            else if (value is long num) numero = num;
            else throw new InvalidCastException();

            if (numero >= (10 ^ 13))
                return numero.ToString("00.000.000/0000-00");
            else if (numero >= (10 ^ 10))
                return numero.ToString("000.000.000-00");
            else
                return numero.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
