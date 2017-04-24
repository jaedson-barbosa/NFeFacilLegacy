using System;

namespace NFeFacil.Sincronizacao.Servidor
{
    internal static class SupervisionarOperacao
    {
        public static Retorno Iniciar<Retorno>(Func<Retorno> operacaoSupervisionada, DateTime momentoRequisicao, TipoDado tipo)
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
                    var resultado = operacaoSupervisionada();
                    item.SucessoSolicitacao = true;
                    db.Add(item);
                    db.SaveChanges();
                    return resultado;
                }
                catch (Exception)
                {
                    item.SucessoSolicitacao = false;
                    db.Add(item);
                    db.SaveChanges();
                    throw;
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
