using System;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class DefinirDataVenda : ContentDialog
    {
        internal DateTimeOffset Data { get; private set; }

        public DefinirDataVenda()
        {
            this.InitializeComponent();
        }
    }
}
