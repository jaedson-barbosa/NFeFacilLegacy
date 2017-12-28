using NFeFacil.ItensBD.Produto;
using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoII
{
    sealed class Processamento : ProcessamentoImposto
    {
        IDadosII dados;

        public override ImpostoBase[] Processar(DetalhesProdutos prod)
        {
            var imposto = dados.Imposto;
            return new ImpostoBase[1] { imposto };
        }

        public override bool ValidarDados()
        {
            var log = Popup.Current;
            var imposto = dados?.Imposto;
            if (imposto != null)
            {
                if (string.IsNullOrEmpty(imposto.vBC))
                {
                    log.Escrever(TitulosComuns.Atenção, "O valor da base de cálculo é obrigatório.");
                }
                else if (string.IsNullOrEmpty(imposto.vDespAdu))
                {
                    log.Escrever(TitulosComuns.Atenção, "O valor das despesas aduaneiras é obrigatório.");
                }
                else if (string.IsNullOrEmpty(imposto.vII))
                {
                    log.Escrever(TitulosComuns.Atenção, "É necessário que o valor do II seja informado.");
                }
                else if (string.IsNullOrEmpty(imposto.vIOF))
                {
                    log.Escrever(TitulosComuns.Atenção, "O valor do imposto sobre operações financeiras deve ser informado.");
                }
                else
                {
                    return true;
                }
            }
            else
            {
                log.Escrever(TitulosComuns.Erro, "Erro na obtenção dos dados do imposto.");
            }
            return false;
        }

        public override void ProcessarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento && Tela?.GetType() == typeof(Detalhar))
            {
                dados = (IDadosII)Tela;
            }
            else if (Detalhamento is DadoPronto pronto)
            {
                ProcessarDadosProntos(pronto.ImpostoPronto);
            }
        }

        protected override void ProcessarDadosProntos(ImpostoArmazenado imposto) { }
    }
}
