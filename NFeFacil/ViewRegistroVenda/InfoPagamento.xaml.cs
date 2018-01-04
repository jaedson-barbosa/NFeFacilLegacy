using System;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class InfoPagamento : ContentDialog
    {
        internal DateTimeOffset Prazo { get; private set; } = DefinicoesTemporarias.DateTimeOffsetNow;
        internal string FormaPagamento => FormaPagamentoEspecial ?? FormaPagamentoIntermediada;

        string formaPagamentoIntermediada;
        string FormaPagamentoIntermediada
        {
            get => formaPagamentoIntermediada;
            set
            {
                formaPagamentoIntermediada = value;
                if (value == "Outro")
                {
                    txt.IsEnabled = true;
                    FormaPagamentoEspecial = string.Empty;
                }
                else
                {
                    txt.IsEnabled = false;
                    txt.Text = string.Empty;
                    FormaPagamentoEspecial = null;
                }
            }
        }

        string FormaPagamentoEspecial { get; set; }

        public InfoPagamento()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrEmpty(FormaPagamento)) args.Cancel = true;
        }
    }
}
