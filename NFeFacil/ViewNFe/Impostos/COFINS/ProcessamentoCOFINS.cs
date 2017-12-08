using NFeFacil.Log;
using System;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.COFINS
{
    public struct ProcessamentoCOFINS : IProcessamentoImposto
    {
        public object Tela { private get; set; }
        public IDetalhamentoImposto Detalhamento { private get; set; }
        delegate object Processador();
        IDadosCOFINS dados;

        public object Processar()
        {
            throw new NotImplementedException();
        }

        public bool ValidarDados(ILog log) => dados.Validar(log);

        public bool ValidarEntradaDados(ILog log)
        {
            if (Detalhamento is DetalhamentoCOFINS detalhamento)
            {
                var valida = (AssociacoesSimples.COFINS.ContainsKey(detalhamento.CST)
                    && AssociacoesSimples.COFINS[detalhamento.CST] == Tela.GetType())
                    || AssociacoesSimples.COFINSPadrao == Tela.GetType();
                if (valida)
                {
                    if (Tela is DetalharCOFINSAliquota aliq)
                    {
                        dados = new DadosAliq();
                    }
                    else if (Tela is DetalharCOFINSQtde valor)
                    {
                        dados = new DadosQtde();
                    }
                    else if (Tela is DetalharCOFINSST st)
                    {
                        dados = new DadosST();
                    }
                    else if (Tela is DetalharCOFINSOutro outr)
                    {
                        dados = new DadosOutr();
                    }
                    else
                    {
                        dados = new DadosNT();
                    }
                }
            }
            return false;
        }

        interface IDadosCOFINS
        {
            bool Validar(ILog log);
        }

        struct DadosAliq : IDadosCOFINS
        {

        }

        struct DadosNT : IDadosCOFINS
        {

        }

        struct DadosQtde : IDadosCOFINS
        {

        }

        struct DadosST : IDadosCOFINS
        {

        }

        struct DadosOutr : IDadosCOFINS
        {

        }
    }
}
