using System;

namespace Banco.ItensBD
{
    public sealed class MotoristaDI
    {
        public Guid Id { get; set; }
        public DateTime UltimaData { get; set; }

        public Guid Veiculo { get; set; }

        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string InscricaoEstadual { get; set; }
        public string XEnder { get; set; }
        public string XMun { get; set; }
        public string UF { get; set; }
    }
}
