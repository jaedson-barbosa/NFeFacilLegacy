using System.Collections.Generic;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos
{
    public interface IImpostosUnidos
    {
        IEnumerable<Imposto> SepararImpostos();
    }
}
