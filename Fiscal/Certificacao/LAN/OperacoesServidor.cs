using Fiscal.Certificacao.LAN.Pacotes;
using Fiscal.Certificacao.LAN.Primitivos;
using BaseGeral.ModeloXML.PartesAssinatura;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using BaseGeral;
using Windows.Storage;

namespace Fiscal.Certificacao.LAN
{
    public struct OperacoesServidor
    {
        public async Task<List<CertificadoExibicao>> ObterCertificados()
        {
            using (var cliente = new HttpClient())
            {
                var uri = new Uri("http://localhost:1010/ObterCertificados");
                var resposta = await cliente.GetAsync(uri);
                var xmlResposta = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                return xmlResposta.FromXElement<CertificadosExibicaoDTO>().Registro;
            }
        }

        public async Task<Assinatura> AssinarRemotamente(CertificadoAssinaturaDTO envio)
        {
            using (var cliente = new HttpClient())
            {
                var file = await ApplicationData.Current.TemporaryFolder.GetFileAsync("Data");
                envio.ToXElement<CertificadoAssinaturaDTO>().Save(file.Path);
                var uri = new Uri("http://localhost:1010/AssinarRemotamente/" + file.Path);
                var resposta = await cliente.GetAsync(uri);
                var xmlResposta = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                return xmlResposta.FromXElement<Assinatura>();
            }
        }
    }
}
