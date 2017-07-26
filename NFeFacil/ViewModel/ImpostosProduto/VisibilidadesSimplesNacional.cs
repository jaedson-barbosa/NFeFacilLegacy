namespace NFeFacil.ViewModel.ImpostosProduto
{
    static class VisibilidadesSimplesNacional
    {
        static readonly VisibilidadeSimplesNacional[] Visibilidades = new VisibilidadeSimplesNacional[]
        {
                new VisibilidadeSimplesNacional(false, false, true),
                new VisibilidadeSimplesNacional(false, false, false),
                new VisibilidadeSimplesNacional(false, false, false),
                new VisibilidadeSimplesNacional(false, true, true),
                new VisibilidadeSimplesNacional(false, true, false),
                new VisibilidadeSimplesNacional(false, true, false),
                new VisibilidadeSimplesNacional(false, false, false),
                new VisibilidadeSimplesNacional(false, false, false),
                new VisibilidadeSimplesNacional(false, false, false),
                new VisibilidadeSimplesNacional(true, true, true)
        };

        public static VisibilidadeSimplesNacional Buscar(int csosn)
        {
            switch (csosn)
            {
                case 101:
                    return Visibilidades[0];
                case 102:
                    return Visibilidades[1];
                case 103:
                    return Visibilidades[2];
                case 201:
                    return Visibilidades[3];
                case 202:
                    return Visibilidades[4];
                case 203:
                    return Visibilidades[5];
                case 300:
                    return Visibilidades[6];
                case 400:
                    return Visibilidades[7];
                case 500:
                    return Visibilidades[8];
                case 900:
                    return Visibilidades[9];
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}
