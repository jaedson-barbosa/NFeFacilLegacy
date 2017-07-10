using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public static class NotasFiscais
    {
        public static int ObterNovoNumero(long cnpjEmitente, ushort serieNota)
        {
            using (var Contexto = new AplicativoContext())
            {
                return (from nota in Contexto.NotasFiscais
                        where nota.CNPJEmitente == cnpjEmitente.ToString()
                        where nota.SerieNota == serieNota
                        select nota.NumeroNota).Max() + 1;
            }
        }
    }
}
