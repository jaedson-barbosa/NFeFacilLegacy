using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMSUFDest
{
    sealed class Processamento : ProcessamentoImposto
    {
        IDadosICMSUFDest dados;

        public override IImposto[] Processar(DetalhesProdutos prod)
        {
            var imposto = dados.Imposto;
            return new IImposto[1] { imposto };
        }

        public override bool ValidarDados(ILog log) => true;

        public override bool ValidarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento
                && Tela?.GetType() == typeof(Detalhar))
            {
                dados = (IDadosICMSUFDest)Tela;
                return true;
            }
            return false;
        }
    }
}
