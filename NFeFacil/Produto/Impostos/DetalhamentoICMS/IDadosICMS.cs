﻿using NFeFacil.ModeloXML.PartesDetalhes;

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS
{
    interface IDadosICMS
    {
        object Processar(DetalhesProdutos prod);
    }
}
