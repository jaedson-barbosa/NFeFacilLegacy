using System;

namespace BibliotecaCentral.Log
{
    public static class Titulos
    {
        public static string ObterString(TitulosComuns título)
        {
            switch (título)
            {
                case TitulosComuns.Sucesso:
                    return "Sucesso";
                case TitulosComuns.ErroSimples:
                    return "Erro simples";
                case TitulosComuns.ErroCatastrófico:
                    return "Erro catastrófico";
                case TitulosComuns.Iniciando:
                    return "Iniciando";
                case TitulosComuns.Lendo:
                    return "Lendo";
                case TitulosComuns.Processando:
                    return "Processando";
                case TitulosComuns.Finalizando:
                    return "Finalizando";
                case TitulosComuns.Log:
                    return nameof(Log);
                case TitulosComuns.ValidaçãoConcluída:
                    return "Tudo certo";
                case TitulosComuns.OperaçãoCancelada:
                    return "Operação cancelada";
                default:
                    throw new Exception($"Existe um título comum que não foi analisado: {título}.");
            }
        }
    }

    public enum TitulosComuns
    {
        Sucesso,
        ErroSimples,
        ErroCatastrófico,
        Iniciando,
        Lendo,
        Processando,
        Finalizando,
        Log,
        ValidaçãoConcluída,
        OperaçãoCancelada
    }
}
