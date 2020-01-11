using System;
using System.Collections.Generic;

namespace BaseGeral.ItensBD
{
    public sealed class Estoque : IUltimaData, IGuidId
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        const string MarcaInatividade = "#INATIVO#";
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool IsAtivo
        {
            get => LocalizacaoGenerica.StartsWith(MarcaInatividade);
            set
            {
                if (value) LocalizacaoGenerica.Insert(0, MarcaInatividade);
                else LocalizacaoGenerica.Remove(0, MarcaInatividade.Length);
            }
        }

        public string LocalizacaoGenerica { get; set; }

        public string Segmento { get; set; }
        public string Prateleira { get; set; }
        public string Locação { get; set; }

        public List<AlteracaoEstoque> Alteracoes { get; set; }
    }
}
