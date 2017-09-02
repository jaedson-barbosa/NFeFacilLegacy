using System.Linq;

namespace NFeFacil.Repositorio
{
    public static class NotasFiscais
    {
        public static int ObterNovoNumero(long cnpjEmitente, ushort serieNota, bool homologacao)
        {
            using (var Contexto = new AplicativoContext())
            {
                return (from nota in Contexto.NotasFiscais
                        where nota.CNPJEmitente == cnpjEmitente.ToString()
                        where nota.SerieNota == serieNota
                        where homologacao && nota.NomeCliente.Trim().ToUpper() == "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL"
                        select nota.NumeroNota).Max() + 1;
            }
        }
    }
}
