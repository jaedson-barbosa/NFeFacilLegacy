using System.Collections.Generic;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public interface IImpostosUnidos
    {
        IEnumerable<IImposto> SepararImpostos();
    }
}
