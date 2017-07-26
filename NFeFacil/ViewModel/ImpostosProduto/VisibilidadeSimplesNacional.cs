namespace NFeFacil.ViewModel.ImpostosProduto
{
    struct VisibilidadeSimplesNacional
    {
        public bool GrupoInicio { get; }
        public bool ICMSST { get; }
        public bool GrupoFim { get; }

        public VisibilidadeSimplesNacional(bool grupoInicio, bool ICMSST, bool grupoFim)
        {
            GrupoInicio = grupoInicio;
            this.ICMSST = ICMSST;
            GrupoFim = grupoFim;
        }
    }
}
