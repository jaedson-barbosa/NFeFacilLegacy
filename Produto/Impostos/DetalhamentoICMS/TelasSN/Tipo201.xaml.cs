using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoICMS.TelasSN
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo201 : UserControl, IProcessamentoImposto
    {
        public int modBCST { get; private set; }
        public string pMVAST { get; private set; }
        public string pRedBCST { get; private set; }
        public double pICMSST { get; private set; }

        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }

        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMS;

        public Tipo201(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var dados = new DadosSN.Tipo201(this)
            {
                CSOSN = Detalhamento.TipoICMSSN,
                Origem = Detalhamento.Origem
            };
            var imposto = new ICMS
            {
                Corpo = (ComumICMS)dados.Processar(prod)
            };
            return new IImposto[1] { imposto };
        }
    }
}
