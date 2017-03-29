using System;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ItensBD
{
    public sealed class ProdutoDI : DadosBaseProdutoOuServico, IId, IConverterDI<DadosBaseProdutoOuServico>
    {
        public int Id { get; set; }
        public ProdutoDI() { }
        public ProdutoDI(DadosBaseProdutoOuServico prod) : base(prod) { }

        public IId Converter(DadosBaseProdutoOuServico item) => new ProdutoDI(item);
    }
}
