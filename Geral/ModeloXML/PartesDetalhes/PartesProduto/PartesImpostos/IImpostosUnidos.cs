using System.Collections.Generic;

namespace NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public interface IImpostosUnidos
    {
        IEnumerable<ImpostoBase> SepararImpostos();
    }
}
