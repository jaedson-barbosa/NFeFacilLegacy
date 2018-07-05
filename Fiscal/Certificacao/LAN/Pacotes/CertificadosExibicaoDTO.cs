using Fiscal.Certificacao.LAN.Primitivos;
using System.Collections.Generic;

namespace Fiscal.Certificacao.LAN.Pacotes
{
    public struct CertificadosExibicaoDTO
    {
        public List<CertificadoExibicao> Registro { get; set; }

        public CertificadosExibicaoDTO(int capacidade)
        {
            Registro = new List<CertificadoExibicao>(capacidade);
        }
    }
}
