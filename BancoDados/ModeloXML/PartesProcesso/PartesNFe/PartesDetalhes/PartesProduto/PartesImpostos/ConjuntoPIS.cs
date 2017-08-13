using System.Collections.Generic;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ConjuntoPIS : IImpostosUnidos
    {
        public PIS PIS = new PIS();
        public PISST PISST = new PISST();

        public IEnumerable<Imposto> SepararImpostos()
        {
            yield return PIS;
            yield return PISST;
        }
    }
}
