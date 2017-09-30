using NFeFacil.IBGE;
using System;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe
{
    internal sealed class ChaveAcesso
    {
        private Detalhes detalhes;
        internal ChaveAcesso(Detalhes detalhes)
        {
            this.detalhes = detalhes;
        }

        internal string CriarChaveAcesso()
        {
            var codigoUF = Estados.Buscar(detalhes.emitente.Endereco.SiglaUF).Codigo;
            var dhEmissao = Convert.ToDateTime(detalhes.identificacao.DataHoraEmissão).ToString("yyMM");
            var CNPJEmitente = detalhes.emitente.CNPJ;
            var modeloIdentificacao = detalhes.identificacao.Modelo;
            var serie = detalhes.identificacao.Serie.ToString().PadLeft(3, '0');
            var numero = detalhes.identificacao.Numero.ToString().PadLeft(9, '0');
            var tipoEmissao = detalhes.identificacao.TipoEmissão;

            if (detalhes.identificacao.ChaveNF == default(long))
            {
                var random = new Random();
                detalhes.identificacao.ChaveNF = random.Next(10000000, 100000000);
            }
            var randomico = detalhes.identificacao.ChaveNF;
            var chave = $"{codigoUF}{dhEmissao}{CNPJEmitente}{modeloIdentificacao}{serie}{numero}{tipoEmissao}{randomico}";

            var dv = detalhes.identificacao.DígitoVerificador = CalcularDV(chave);
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
