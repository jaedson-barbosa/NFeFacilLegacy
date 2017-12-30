using System;

namespace NFeFacil.ItensBD
{
    public sealed class Inutilizacao
    {
        public string Id { get; set; }
        public string CNPJ { get; set; }
        public bool Homologacao { get; set; }
        public int InicioRange { get; set; }
        public int Serie { get; set; }
        public int FimRange { get; set; }
        public long NumeroProtocolo { get; set; }
        public DateTime MomentoProcessamento { get; set; }

        public string ExibicaoMomentoProcessamento => MomentoProcessamento.ToString("dd/MM/yyyy hh:mm:ss");
    }
}
