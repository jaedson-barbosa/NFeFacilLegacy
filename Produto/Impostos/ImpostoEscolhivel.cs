// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos
{
    public sealed class ImpostoEscolhivel
    {
        public ImpostoEscolhivel(ImpostoArmazenado template)
        {
            Id = 0;
            Template = template;
        }

        public int Id { get; set; }
        public ImpostoArmazenado Template { get; set; }
    }
}
