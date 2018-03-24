﻿using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos.DetalhamentoII
{
    [View.DetalhePagina("Imposto de importação")]
    public sealed partial class Detalhar : Page, IDadosII
    {
        public II Imposto { get; } = new II();

        public Detalhar()
        {
            InitializeComponent();
        }
    }
}
