using Comum.Primitivos;
using System.Collections.Generic;

namespace Comum.Pacotes
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
