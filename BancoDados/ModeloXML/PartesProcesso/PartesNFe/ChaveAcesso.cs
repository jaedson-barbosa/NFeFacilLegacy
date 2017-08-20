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
            var dhEmissao = Convert.ToDateTime(detalhes.identificação.DataHoraEmissão).ToString("yyMM");
            var CNPJEmitente = detalhes.emitente.CNPJ;
            var modeloIdentificacao = detalhes.identificação.Modelo;
            var serie = detalhes.identificação.Serie.ToString().PadLeft(3, '0');
            var numero = detalhes.identificação.Numero.ToString().PadLeft(9, '0');
            var tipoEmissao = detalhes.identificação.TipoEmissão;

            if (detalhes.identificação.ChaveNF == default(long))
            {
                var random = new Random();
                detalhes.identificação.ChaveNF = random.Next(10000000, 100000000);
            }
            var randomico = detalhes.identificação.ChaveNF;
            var chave = $"{codigoUF}{dhEmissao}{CNPJEmitente}{modeloIdentificacao}{serie}{numero}{tipoEmissao}{randomico}";

            var dv = detalhes.identificação.DígitoVerificador = CalcularDV(chave);
            return chave + dv;
        }

        private static int CalcularDV(string chave)
        {
            int soma = 0; // Vai guardar a Soma
            sushort peso = 2; // vai guardar o peso de multiplicacao
            //percorrendo cada caracter da chave da direita para esquerda para fazer os calculos com o pesso
            for (int i = chave.Length - 1; i >= 0; i--, peso++)
            {
                if (peso == 10) peso = 2;
                sushort atual = sushort.Parse(chave[i].ToString());
                soma += atual * peso;
            }
            //Agora que tenho a soma vamos pegar o resto da divisão por 11
            var resto = soma % 11;
            //Aqui temos uma regrinha, se o resto da divisão for 0 ou 1 então o dv vai ser 0
            return (resto == 0 || resto == 1) ? 0 : 11 - resto;
        }
    }
}
