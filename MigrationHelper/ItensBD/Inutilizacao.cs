using System;

namespace BaseGeral.ItensBD
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
        public string XMLCompleto { get; set; }

        public bool IsNFCe { get; set; }

        public string ExibicaoMomentoProcessamento => MomentoProcessamento.ToString("dd/MM/yyyy hh:mm:ss");
    }
}
