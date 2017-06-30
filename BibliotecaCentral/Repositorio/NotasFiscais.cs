using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class NotasFiscais : ConexaoBanco
    {
        public IEnumerable<NFeDI> Registro => from not in Contexto.NotasFiscais
                                              orderby not.DataEmissao descending
                                              select not;

        public void Adicionar(NFeDI nota)
        {
            nota.UltimaData = DateTime.Now;
            var add = Contexto.Add(nota);
        }

        public void Atualizar(NFeDI nota)
        {
            nota.UltimaData = DateTime.Now;
            Contexto.Update(nota);
        }

        public void Remover(NFeDI nota)
        {
            Contexto.Remove(nota);
        }

        public int ObterNovoNumero(long cnpjEmitente, ushort serieNota)
        {
            return (from nota in Contexto.NotasFiscais
                    where nota.CNPJEmitente == cnpjEmitente.ToString()
                    where nota.SerieNota == serieNota
                    select nota.NumeroNota).Max() + 1;
        }
    }
}
