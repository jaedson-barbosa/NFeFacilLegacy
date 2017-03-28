using NFeFacil.ItensBD;

namespace NFeFacil
{
    internal struct GrupoViewBanco<TipoDado> where TipoDado : IId
    {
        public TipoDado ItemBanco { get; set; }
        public TipoOperacao OperacaoRequirida { get; set; }
    }

    enum TipoOperacao
    {
        Adicao,
        Edicao
    }
}
