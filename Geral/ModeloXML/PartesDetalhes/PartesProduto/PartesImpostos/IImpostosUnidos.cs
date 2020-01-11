using System.Collections.Generic;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public interface IImpostosUnidos
    {
        IEnumerable<IImposto> SepararImpostos();
    }
}
