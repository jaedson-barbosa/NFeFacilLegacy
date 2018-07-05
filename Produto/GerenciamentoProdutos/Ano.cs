using BaseGeral.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.GerenciamentoProdutos
{
    struct Ano
    {
        internal int Atual { get; }
        internal Mes[] Meses { get; set; }
        internal double Total { get; set; }

        internal Ano(IEnumerable<AlteracaoEstoque> alteracoes, int atual)
        {
            Atual = atual;
            Meses = new Mes[12];
            for (int i = 1; i <= 12; i++)
            {
                Meses[i - 1] = new Mes(alteracoes.Where(x => x.MomentoRegistro.Month == i), atual, i);
            }
            Total = Meses.Sum(x => x.Total);
        }

        internal struct Mes
        {
            internal Dia[] Dias { get; set; }
            internal double Total { get; set; }

            internal Mes(IEnumerable<AlteracaoEstoque> alteracoes, int ano, int atual)
            {
                var quantDias = DateTime.DaysInMonth(ano, atual);
                Dias = new Dia[quantDias];
                for (int i = 0; i < quantDias; i++)
                {
                    Dias[i] = new Dia(alteracoes.Where(x => x.MomentoRegistro.Day == i));
                }
                Total = Dias.Sum(x => x.Total);
            }

            internal struct Dia
            {
                internal double Total { get; set; }

                internal Dia(IEnumerable<AlteracaoEstoque> alteracoes)
                {
                    Total = alteracoes.Sum(x => x.Alteração);
                }
            }
        }
    }
}
