using NFeFacil.IBGE;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoNFe
{
    public sealed partial class EnderecoDiferenteExterior : ContentDialog
    {
        public EnderecoDiferenteExterior()
        {
            this.InitializeComponent();
        }

        public RetiradaOuEntrega Endereco { get; } = new RetiradaOuEntrega()
        {
            CodigoMunicipio = 9999999,
            NomeMunicipio = "EXTERIOR",
            SiglaUF = "EX"
        };
        ObservableCollection<Municipio> MunicipiosDoEstado { get; } = new ObservableCollection<Municipio>();

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
