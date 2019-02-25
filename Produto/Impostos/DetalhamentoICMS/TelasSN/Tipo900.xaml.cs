using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoICMS.TelasSN
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo900 : UserControl, IProcessamentoImposto
    {
        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }
        public int modBC { get; private set; }
        public string vBC { get; private set; }
        public string pRedBC { get; private set; }
        public string pICMS { get; private set; }
        public string vICMS { get; private set; }
        public int modBCST { get; private set; }
        public string pMVAST { get; private set; }
        public string pRedBCST { get; private set; }
        public string vBCST { get; private set; }
        public string pICMSST { get; private set; }
        public string vICMSST { get; private set; }

        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMS;

        public Tipo900(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var dados = new DadosSN.Tipo900(this)
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
