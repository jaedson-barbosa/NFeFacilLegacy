﻿using Fiscal.Certificacao.LAN.Pacotes;
using Fiscal.Certificacao.LAN.Primitivos;
using BaseGeral.ModeloXML.PartesAssinatura;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BaseGeral.Certificacao;
using BaseGeral;

namespace Fiscal.Certificacao.LAN
{
    public struct OperacoesServidor
    {
        public static string RootUri => ConfiguracoesCertificacao.Origem == OrigemCertificado.Cliente
            ? ConfiguracoesCertificacao.IPServidorCertificacao : "localhost";

        public async Task<List<CertificadoExibicao>> ObterCertificados()
        {
            var origem = ConfiguracoesCertificacao.Origem;
            using (var cliente = new HttpClient())
            {
                var uri = new Uri($"http://{RootUri}:{(origem == OrigemCertificado.Cliente ? 2020 : 1010)}/ObterCertificados");

                var resposta = await cliente.GetAsync(uri);
                var xmlResposta = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                return xmlResposta.FromXElement<CertificadosExibicaoDTO>().Registro;
            }
        }

        public async Task<Assinatura> AssinarRemotamente(CertificadoAssinaturaDTO envio)
        {
            var origem = ConfiguracoesCertificacao.Origem;
            using (var cliente = new HttpClient())
            {
                var uri = new Uri($"http://{RootUri}:{(origem == OrigemCertificado.Cliente ? 2020 : 1010)}/AssinarRemotamente");
                var xml = envio.ToXElement<CertificadoAssinaturaDTO>().ToString(SaveOptions.DisableFormatting);
                var conteudo = new StringContent(xml, Encoding.UTF8, "text/xml");

                var resposta = await cliente.PostAsync(uri, conteudo);
                var xmlResposta = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                return xmlResposta.FromXElement<Assinatura>();
            }
        }
    }
}
