using Comum.Pacotes;
using Comum.Primitivos;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesAssinatura;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            using (var cliente = new HttpClient())
            {
                var uri = new Uri($"http://{Ip}:1010/ObterCertificados");

                var resposta = await cliente.GetAsync(uri);
                using (var stream = await resposta.Content.ReadAsStreamAsync())
                {
                    return stream.FromXElement<CertificadosExibicaoDTO>().Registro;
                }
            }
        }

        public async Task<Assinatura> AssinarRemotamente(CertificadoAssinaturaDTO envio)
        {
            using (var cliente = new HttpClient())
            {
                var uri = new Uri($"http://{Ip}:1010/AssinarRemotamente");
                var xml = envio.ToXElement<CertificadoAssinaturaDTO>().ToString(SaveOptions.DisableFormatting);
                var conteudo = new StringContent(xml, Encoding.UTF8, "text/xml");

                var resposta = await cliente.PostAsync(uri, conteudo);
                var str = await resposta.Content.ReadAsStringAsync();
                using (var stream = await resposta.Content.ReadAsStreamAsync())
                {
                    return stream.FromXElement<Assinatura>();
                }
            }
        }
    }
}
