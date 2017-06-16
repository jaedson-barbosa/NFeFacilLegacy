using Comum.Pacotes;
using Comum.Primitivos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BibliotecaCentral.Certificacao.LAN
{
    public struct OperacoesServidor
    {
        string ip;

        public OperacoesServidor(string ip)
        {
            this.ip = ip;
        }

        public async Task<List<CertificadoExibicao>> ObterCertificados()
        {
            var dto = await EnviarRequisicao<CertificadosExibicaoDTO>("ObterCertificados");
            return dto.Registro;
        }

        public async Task<CertificadoAssinatura> ObterCertificado(string serial)
        {
            var dto = await EnviarRequisicao<CertificadoAssinaturaDTO>("ObterCertificados", serial);
            return (CertificadoAssinatura)dto;
        }

        async Task<T> EnviarRequisicao<T>(params string[] parametros)
        {
            using (var cliente = new HttpClient())
            {
                StringBuilder construtorCaminho = new StringBuilder($"http://{ip}:8080");
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
