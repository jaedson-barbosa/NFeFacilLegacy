using System;
using System.Collections.Generic;

namespace NFeFacil.ItensBD
{
    public sealed class Estoque
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public string LocalizacaoGenerica { get; set; }

        public string Segmento { get; set; }
        public string Prateleira { get; set; }
        public string Locação { get; set; }

        public List<AlteracaoEstoque> Alteracoes { get; set; }
    }
}
