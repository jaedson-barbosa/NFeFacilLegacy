using BaseGeral.IBGE;
using System;

namespace BaseGeral.ModeloXML
{
    internal sealed class ChaveAcesso
    {
        private InformacoesBase detalhes;
        internal ChaveAcesso(InformacoesBase detalhes)
        {
            this.detalhes = detalhes;
        }

        internal string CriarChaveAcesso()
        {
            var identificacao = detalhes.identificacao;
            var codigoUF = Estados.Buscar(detalhes.Emitente.Endereco.SiglaUF).Codigo;
            var dhEmissao = Convert.ToDateTime(identificacao.DataHoraEmissão).ToString("yyMM");
            var CNPJEmitente = detalhes.Emitente.CNPJ;
            var modeloIdentificacao = identificacao.Modelo;
            var serie = identificacao.Serie.ToString().PadLeft(3, '0');
            var numero = identificacao.Numero.ToString().PadLeft(9, '0');
            var tipoEmissao = identificacao.TipoEmissão;

            if (identificacao.ChaveNF == default(int))
            {
                var random = new Random();
                identificacao.ChaveNF = random.Next(10000000, 100000000);
            }
            var randomico = identificacao.ChaveNF;
            var chave = $"{codigoUF}{dhEmissao}{CNPJEmitente}{modeloIdentificacao}{serie}{numero}{tipoEmissao}{randomico}";

            var dv = identificacao.DígitoVerificador = CalcularDV(chave);
            return chave + dv;
        }

        private static int CalcularDV(string chave)
        {
            int soma = 0; // Vai guardar a Soma
            sbyte peso = 2; // vai guardar o peso de multiplicacao
            //percorrendo cada caracter da chave da direita para esquerda para fazer os calculos com o pesso
            for (int i = chave.Length - 1; i >= 0; i--, peso++)
            {
                if (peso == 10) peso = 2;
                sbyte atual = sbyte.Parse(chave[i].ToString());
                soma += atual * peso;
            }
            //Agora que tenho a soma vamos pegar o resto da divisão por 11
            var resto = soma % 11;
            //Aqui temos uma regrinha, se o resto da divisão for 0 ou 1 então o dv vai ser 0
            return (resto == 0 || resto == 1) ? 0 : 11 - resto;
        }
    }
}
