using System;
using NFeFacil.ItensBD;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static NFeFacil.ExtensoesPrincipal;
using Windows.UI.Xaml;

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

        public DARV2()
        {
            this.InitializeComponent();
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
            if (value is string str)
            {
                return AplicarMascaraDocumento(str);
            }
            else if (value is long num)
            {
                return AplicarMascaraDocumento(num.ToString());
            }
            throw new System.InvalidCastException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        string AplicarMascaraDocumento(string original)
        {
            original = original.Trim();
            if (original.Length == 14)
            {
                // É CNPJ
                return $"{original.Substring(0, 2)}.{original.Substring(2, 3)}.{original.Substring(5, 3)}/{original.Substring(8, 4)}.{original.Substring(12, 2)}";
            }
            else if (original.Length == 11)
            {
                // É CPF
                return $"{original.Substring(0, 3)}.{original.Substring(3, 3)}.{original.Substring(6, 3)}-{original.Substring(9, 2)}";
            }
            else
            {
                // Não é nem CNPJ nem CPF
                return original;
            }
        }
    }
}
