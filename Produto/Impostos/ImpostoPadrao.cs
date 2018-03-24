// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos
{
    sealed class ImpostoPadrao : ImpostoArmazenado
    {
        public ImpostoPadrao(PrincipaisImpostos tipo)
        {
            Tipo = tipo;
            NomeTemplate = "Template padrão";
        }
    }
}
