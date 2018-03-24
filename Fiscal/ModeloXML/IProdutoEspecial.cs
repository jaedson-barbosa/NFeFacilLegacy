using System.Collections.Generic;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;

namespace BaseGeral.ModeloXML
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