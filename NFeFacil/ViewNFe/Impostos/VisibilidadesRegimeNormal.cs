namespace NFeFacil.ViewNFe.Impostos
{
    static class VisibilidadesRegimeNormal
    {
        static readonly VisibilidadeRegimeNormal[] Visibilidades = new VisibilidadeRegimeNormal[]
        {
                new VisibilidadeRegimeNormal(false, true, false, false, false, false),
                new VisibilidadeRegimeNormal(false, true, true, false, false, false),
                new VisibilidadeRegimeNormal(true, true, true, false, true, false, false, true),
                new VisibilidadeRegimeNormal(true, true, false, false, true, false),
                new VisibilidadeRegimeNormal(false, false, true, false, true, false),
                new VisibilidadeRegimeNormal(false, false, false, false, true, false),
                new VisibilidadeRegimeNormal(false, false, false, false, true, false),
                new VisibilidadeRegimeNormal(false, false, false, true, false, false, true),
                new VisibilidadeRegimeNormal(false, false, false, false, true, false),
                new VisibilidadeRegimeNormal(true, true, false, false, false, true),
                new VisibilidadeRegimeNormal(false, false, false, true, false, false),
                new VisibilidadeRegimeNormal(true, true, true, false, true, false),
                new VisibilidadeRegimeNormal(true, true, true, false, true, false),
                new VisibilidadeRegimeNormal(true, true, true, false, true, false, false, true)
        };

        public static VisibilidadeRegimeNormal Buscar(int cst)
        {
            switch (cst)
            {
                case 0:
                    return Visibilidades[0];
                case 10:
                    return Visibilidades[1];
                case 1010:
                    return Visibilidades[2];
                case 20:
                    return Visibilidades[3];
                case 30:
                    return Visibilidades[4];
                case 40:
                    return Visibilidades[5];
                case 41:
                    return Visibilidades[6];
                case 4141:
                    return Visibilidades[7];
                case 50:
                    return Visibilidades[8];
                case 51:
                    return Visibilidades[9];
                case 60:
                    return Visibilidades[10];
                case 70:
                    return Visibilidades[11];
                case 90:
                    return Visibilidades[12];
                case 9090:
                    return Visibilidades[13];
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}
