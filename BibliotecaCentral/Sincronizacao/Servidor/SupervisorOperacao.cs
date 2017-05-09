using System;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    internal static class SupervisorOperacao
    {
        public static Retorno Supervisionar<Retorno>(Func<Retorno> operacao, DateTime momentoRequisicao, TipoDado tipo)
        {
            using (var db = new AplicativoContext())
            {
                var item = new ItensBD.ResultadoSincronizacaoServidor()
                {
                    MomentoRequisicao = momentoRequisicao,
                    TipoDadoSolicitado = (int)tipo
                };
                try
                {
                    var resultado = operacao();
                    item.SucessoSolicitacao = true;
                    db.Add(item);
                    db.SaveChanges();
                    return resultado;
                }
                catch (Exception e)
                {
                    item.SucessoSolicitacao = false;
                    db.Add(item);
                    db.SaveChanges();
                    throw e;
                }
            }
        }
    }

    internal enum TipoDado
    {
        DadoBase,
        NotaFiscal,
        Configuracao,
        SenhaDeAcesso
    }
}
