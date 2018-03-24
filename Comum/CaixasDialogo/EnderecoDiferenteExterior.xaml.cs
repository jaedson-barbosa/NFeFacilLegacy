using BaseGeral.ModeloXML;
using BaseGeral.ModeloXML.PartesDetalhes;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal.ViewNFe.CaixasDialogo
{
    public sealed partial class EnderecoDiferenteExterior : ContentDialog
    {
        public EnderecoDiferenteExterior()
        {
            InitializeComponent();
        }

        public RetiradaOuEntrega Endereco { get; } = new RetiradaOuEntrega()
        {
            CodigoMunicipio = 9999999,
            NomeMunicipio = "EXTERIOR",
            SiglaUF = "EX"
        };

        TiposDocumento tipoDocumento;
        public int TipoDocumento
        {
            get => (int)(tipoDocumento = string.IsNullOrEmpty(Endereco.CNPJ) ? TiposDocumento.CPF : TiposDocumento.CNPJ);
            set => tipoDocumento = (TiposDocumento)value;
        }

        public string Documento
        {
            get
            {
                return tipoDocumento == TiposDocumento.CNPJ ? Endereco.CNPJ : Endereco.CPF;
            }
            set
            {
                if (tipoDocumento == TiposDocumento.CPF)
                {
                    Endereco.CNPJ = null;
                    Endereco.CPF = value;
                }
                else
                {
                    Endereco.CPF = null;
                    Endereco.CNPJ = value;
                }
            }
        }
    }
}
