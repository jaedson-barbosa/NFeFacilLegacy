using System;

namespace BaseGeral.ItensBD
{
    class Fornecedor : IStatusAtivacao, IGuidId
    {
        public bool Ativo { get; set; } = true;
        public DateTime UltimaData { get; set; }
        public Guid Id { get; set; }


    }
}
