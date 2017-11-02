using Comum.Pacotes;
using Comum.Primitivos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NFeFacil.Certificacao.LAN
{
    public struct OperacoesServidor
    {
        string ip;
        public string Ip
        {
            get
            {
                if (string.IsNullOrEmpty(ip))
                {
                    ip = ConfiguracoesCertificacao.IPServidorCertificacao;
                }
                return ip;
            }
            set => ip = value;
        }

        public OperacoesServidor(string ip)
        {
            this.ip = ip;
        }

        public async Task<List<CertificadoExibicao>> ObterCertificados()
        {
            var dto = await EnviarRequisicao<CertificadosExibicaoDTO>(Comum.NomesMetodos.ObterCertificados);
            return dto.Registro;
        }

        public async Task<CertificadoAssinatura> ObterCertificado(string serial)
        {
            var dto = await EnviarRequisicao<CertificadoAssinaturaDTO>(Comum.NomesMetodos.ObterChaveCertificado, serial);
            return (CertificadoAssinatura)dto;
        }

        async Task<T> EnviarRequisicao<T>(params string[] parametros)
        {
            using (var cliente = new HttpClient())
            {
                StringBuilder construtorCaminho = new StringBuilder($"http://{Ip}:8080");
                for (int i = 0; i < parametros.Length; i++)
                {
                    construtorCaminho.Append('/');
                    construtorCaminho.Append(parametros[i]);
                }

                var resposta = await cliente.GetAsync(new Uri(construtorCaminho.ToString()));
                using (var stream = await resposta.Content.ReadAsStreamAsync())
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
                }
            }
        }
    }
}
