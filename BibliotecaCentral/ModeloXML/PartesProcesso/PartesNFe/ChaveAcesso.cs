using BibliotecaCentral.IBGE;
using System;
using System.Linq;
using System.Text;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe
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
            var construtor = new StringBuilder();
            var codigoUF = Estados.EstadosCache.Single(x => x.Sigla == detalhes.emitente.endereco.SiglaUF).Codigo;
            var dhEmissao = Convert.ToDateTime(detalhes.identificação.DataHoraEmissão).ToString("yyMM");
            var CNPJEmitente = detalhes.emitente.CNPJ;
            var modeloIdentificacao = detalhes.identificação.Modelo;
            var serie = detalhes.identificação.Serie.ToString().PadLeft(3, '0');
            var numero = detalhes.identificação.Numero.ToString().PadLeft(9, '0');
            var tipoEmissao = detalhes.identificação.TipoEmissão;

            var random = new Random();
            var randomico = detalhes.identificação.ChaveNF = $"{random.Next(100, 1000)}{random.Next(100, 1000)}{random.Next(10, 100)}";
            construtor.Append(codigoUF);
            construtor.Append(dhEmissao);
            construtor.Append(CNPJEmitente);
            construtor.Append(modeloIdentificacao);
            construtor.Append(serie);
            construtor.Append(numero);
            construtor.Append(tipoEmissao);
            construtor.Append(randomico);

            var dv = CalcularDV(construtor);
            construtor.Append(dv);
            return construtor.ToString();
        }

        private static int CalcularDV(StringBuilder chave)
        {
            var soma = 0; // Vai guardar a Soma
            var mod = -1; // Vai guardar o Resto da divisão
            var peso = 2; // vai guardar o peso de multiplicacao
            //percorrendo cada caracter da chave da direita para esquerda para fazer os calculos com o pesso
            for (int i = chave.Length - 1; i != -1; i--)
            {
                soma += chave[i] * peso;
                //sempre que for 9 voltamos o pesso a 2
                if (peso < 9) peso++;
                else peso = 2;
            }
            //Agora que tenho a soma vamos pegar o resto da divisão por 11
            mod = soma % 11;
            //Aqui temos uma regrinha, se o resto da divisão for 0 ou 1 então o dv vai ser 0
            return mod == 0 || mod == 1 ? 0 : 11 - mod;
        }
    }
}
