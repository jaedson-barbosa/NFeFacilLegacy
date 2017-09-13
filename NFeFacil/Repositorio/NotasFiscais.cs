using System.Linq;

namespace NFeFacil.Repositorio
{
    public static class NotasFiscais
    {
        public static int ObterNovoNumero(string cnpjEmitente, ushort serieNota, bool homologacao)
        {
            using (var Contexto = new AplicativoContext())
            {
                return (from nota in Contexto.NotasFiscais
                        where nota.CNPJEmitente == cnpjEmitente.ToString()
                        where nota.SerieNota == serieNota
                        let notaHomologacao = nota.NomeCliente.Trim().ToUpper() == "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL"
                        where homologacao ? notaHomologacao : !notaHomologacao
                        select nota.NumeroNota).Max() + 1;
            }
        }
    }
}
