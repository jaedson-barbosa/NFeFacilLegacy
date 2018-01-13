using System.Collections.Generic;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;

namespace NFeFacil.ModeloXML
{
    public interface IProdutoEspecial
    {
        List<Arma> armas { get; set; }
        Combustivel comb { get; set; }
        List<Medicamento> medicamentos { get; set; }
        string NRECOPI { get; set; }
        VeiculoNovo veicProd { get; set; }
    }
}