using System;

namespace BaseGeral.ItensBD
{
    public sealed class VeiculoDI : IStatusAtivacao, IGuidId
    {
        public Guid Id { get; set; }

        public string Descricao { get; set; }
        public string Placa { get; set; }
        public string UF { get; set; }
        public string RNTC { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime UltimaData { get; set; }
    }
}
