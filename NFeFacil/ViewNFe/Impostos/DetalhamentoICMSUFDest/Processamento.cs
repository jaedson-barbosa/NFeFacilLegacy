using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMSUFDest
{
    sealed class Processamento : ProcessamentoImposto
    {
        IDadosICMSUFDest dados;

        public override Imposto[] Processar(ProdutoOuServico prod)
        {
            var imposto = dados.Imposto;
            return new Imposto[1] { imposto };
        }

        public override bool ValidarDados(ILog log)
        {
            var imposto = dados.Imposto;
            var valido = imposto.IsValido;
            if (!valido)
            {
                log.Escrever(TitulosComuns.Atenção, "Dados inválidos");
            }
            return valido;
        }

        public override bool ValidarEntradaDados(ILog log)
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
