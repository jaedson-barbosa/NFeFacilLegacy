using ServidorCertificacao.Primitivos;
using System.Collections.Generic;

namespace ServidorCertificacao.Pacotes
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
