namespace NFeFacil.ItensBD.Produto
{
    public sealed class ICMSArmazenado : ImpostoArmazenado
    {
        public int ModBC { get; set; }
        public double PICMS { get; set; }

        public int ModBCST { get; set; }
        public string PMVAST { get; set; }
        public string PRedBCST { get; set; }
        public double PICMSST { get; set; }

        public double PRedBC { get; set; }
        public string MotDesICMS { get; set; }

        public double PDif { get; set; }
        public bool Calcular { get; set; }

        public string PCredSN { get; }
        public string VCredICMSSN { get; }
    }
}
