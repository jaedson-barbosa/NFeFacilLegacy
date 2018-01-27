using System.Threading.Tasks;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;

namespace NFeFacil.Fiscal
{
    sealed class AcoesNFCe : AcoesVisualizacao
    {
        public AcoesNFCe(NFeDI nota) : base(nota)
        {

        }

        public override Task Assinar()
        {
            throw new System.NotImplementedException();
        }

        public override void Editar()
        {
            throw new System.NotImplementedException();
        }

        public override Task Exportar()
        {
            throw new System.NotImplementedException();
        }

        public override void Imprimir()
        {
            throw new System.NotImplementedException();
        }

        public override InformacoesBase ObterVisualizacao()
        {
            throw new System.NotImplementedException();
        }

        public override void Salvar()
        {
            throw new System.NotImplementedException();
        }

        public override Task Transmitir()
        {
            throw new System.NotImplementedException();
        }
    }
}
