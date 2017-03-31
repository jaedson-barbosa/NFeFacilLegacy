using NFeFacil.IBGE;
using System;
using System.Linq;
using System.Text;

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
            var construtor = new StringBuilder();
            var estados = Estados.EstadosCache;
            construtor.Append(estados.Single(x => x.Sigla == detalhes.emitente.endereco.SiglaUF).Codigo);
            var DataHoraEmissao = Convert.ToDateTime(detalhes.identificação.DataHoraEmissão);
            construtor.Append($"{DataHoraEmissao.Year.ToString().Substring(2)}{DataHoraEmissao.Month}");
            construtor.Append(detalhes.emitente.CNPJ);
            construtor.Append(detalhes.identificação.Modelo);
            construtor.Append(detalhes.identificação.Serie.ToString().PadLeft(3, '0'));
            construtor.Append(detalhes.identificação.Numero.ToString().PadLeft(9, '0'));
            construtor.Append(detalhes.identificação.TipoEmissão);
            var rnd = new Random();
            construtor.Append($"{rnd.Next(100, 1000)}{rnd.Next(100, 1000)}{rnd.Next(10, 100)}");
            construtor.Append(CalcularDV(construtor));
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
