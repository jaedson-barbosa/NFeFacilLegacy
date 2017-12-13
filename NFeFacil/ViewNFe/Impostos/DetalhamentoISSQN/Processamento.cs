using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoISSQN
{
    public sealed class Processamento : ProcessamentoImposto
    {
        IDadosISSQN dados;

        public override Imposto[] Processar(DetalhesProdutos prod)
        {
            var imposto = dados.Imposto;
            return new Imposto[1] { imposto };
        }

        public override bool ValidarDados(ILog log)
        {
            var imposto = dados.Imposto;
            if (string.IsNullOrEmpty(imposto.vBC))
            {
                log.Escrever(TitulosComuns.Atenção, "Valor da base de cálculo não pode ficar em branco.");
            }
            else if (string.IsNullOrEmpty(imposto.vAliq))
            {
                log.Escrever(TitulosComuns.Atenção, "Alíquota não pode ficar em branco.");
            }
            else if (string.IsNullOrEmpty(imposto.vISSQN))
            {
                log.Escrever(TitulosComuns.Atenção, "Valor do ISSQN não pode ficar em branco.");
            }
            else if (string.IsNullOrEmpty(imposto.cMunFG))
            {
                log.Escrever(TitulosComuns.Atenção, "O município de ocorrência deve ser informado.");
            }
            else if (string.IsNullOrEmpty(imposto.cListServ))
            {
                log.Escrever(TitulosComuns.Atenção, "Item da lista de serviços deve ser informado.");
            }
            else if (string.IsNullOrEmpty(imposto.indISS))
            {
                log.Escrever(TitulosComuns.Atenção, "Indicador da exigibilidade do ISS é obrigatório.");
            }
            else if (string.IsNullOrEmpty(imposto.indIncentivo))
            {
                log.Escrever(TitulosComuns.Atenção, "O indicado de incentivo fiscal deve ser informado.");
            }
            else
            {
                return true;
            }
            return false;
        }

        public override bool ValidarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento
                && AssociacoesSimples.ISSQN[detalhamento.Exterior] == Tela?.GetType())
            {
                dados = (IDadosISSQN)Tela;
                return true;
            }
            return false;
        }
    }
}
