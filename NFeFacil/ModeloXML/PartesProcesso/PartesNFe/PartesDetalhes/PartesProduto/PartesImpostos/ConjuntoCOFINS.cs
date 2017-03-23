﻿using System.Collections.Generic;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ConjuntoCOFINS : IImpostosUnidos
    {
        public COFINS COFINS = new COFINS();
        public COFINSST COFINSST = new COFINSST();

        public IEnumerable<Imposto> SepararImpostos()
        {
            yield return COFINS;
            yield return COFINSST;
        }
    }
}
