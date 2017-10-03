namespace NFeFacil.ViewNFe.CaixasImpostos
{
    struct VisibilidadeRegimeNormal
    {
        public bool PRedBC { get; }
        public bool GrupoInicio { get; }
        public bool ICMSST { get; }
        public bool GrupoMeio { get; }
        public bool IcmsDeson { get; }
        public bool GrupoFim { get; }
        public bool NormalICMSST { get; }
        public bool NormalICMSPart { get; }

        public VisibilidadeRegimeNormal(bool pRedBC, bool grupoInicio, bool ICMSST, bool grupoMeio, bool icmsDeson, bool grupoFim, bool normalICMSST = false, bool normalICMSPart = false)
        {
            PRedBC = pRedBC;
            GrupoInicio = grupoInicio;
            this.ICMSST = ICMSST;
            GrupoMeio = grupoMeio;
            IcmsDeson = icmsDeson;
            GrupoFim = grupoFim;
            NormalICMSST = normalICMSST;
            NormalICMSPart = normalICMSPart;
        }
    }
}
