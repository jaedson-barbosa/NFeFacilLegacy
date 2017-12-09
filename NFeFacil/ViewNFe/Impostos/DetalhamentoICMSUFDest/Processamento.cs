using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMSUFDest
{
    sealed class Processamento : ProcessamentoImposto
    {
        public override Imposto[] Processar(ProdutoOuServico prod)
        {
            var imposto = ((IDadosICMSUFDest)Tela).Imposto;
            return new Imposto[1] { imposto };
        }

        public override bool ValidarDados(ILog log)
        {
            var imposto = ((IDadosICMSUFDest)Tela).Imposto;
            var valido = imposto.IsValido;
            if (!valido)
            {
                log.Escrever(TitulosComuns.Atenção, "Dados inválidos");
            }
            return valido;
        }

        public override bool ValidarEntradaDados(ILog log)
        {
            return Detalhamento is Detalhamento detalhamento
                && Tela.GetType() == typeof(Detalhar);
        }
    }
}
