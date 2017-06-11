using BibliotecaCentral.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage.Pickers;

namespace BibliotecaCentral.Certificacao
{
    public static class ServidorCertificacao
    {
        public async static Task Exportar(ILog log)
        {
            var salvador = new FileSavePicker()
            {
                SuggestedFileName = "Repositorio remoto de certificados",
                DefaultFileExtension = ".zip"
            };
            salvador.FileTypeChoices.Add("Arquivo comprimido", new string[1] { ".zip" });
            var arquivo = await salvador.PickSaveFileAsync();
            if (arquivo != null)
            {
                using (var stream = await arquivo.OpenStreamForWriteAsync())
                {
                    var recurso = new RecursoInserido().Retornar("BibliotecaCentral.Certificacao.RepositorioRemoto.zip");
                    recurso.CopyTo(stream);
                }
                log.Escrever(TitulosComuns.Sucesso, "Arquivo salvo com sucesso, inicie o repositório remoto com o Iniciar.bat");
            }
        }

        public async static Task<bool> Conectar()
        {
            var caixa = new CaixasDialogo.ConectarServidor();
            if (await caixa.ShowAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                var ip = caixa.IP;
                ConfiguracoesCertificacao.IPServidorCertificacao = ip;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Esquecer()
        {
            ConfiguracoesCertificacao.IPServidorCertificacao = null;
        }

        public async static Task<List<CertificadoFundamental>> ObterCertificados()
        {
            var ip = ConfiguracoesCertificacao.IPServidorCertificacao;
            using (var cliente = new HttpClient())
            {
                var resposta = await cliente.GetAsync(new Uri($"http://{ip}:8080/ObterCertificados"));
                using (var stream = await resposta.Content.ReadAsStreamAsync())
                {
                    var conjunto = new XmlSerializer(typeof(CertificadosLAN)).Deserialize(stream) as CertificadosLAN;
                    return conjunto.Certificados;
                }
            }
        }
    }

    [XmlRoot("Certificados")]
    public sealed class CertificadosLAN
    {
        [XmlElement("Certificado")]
        public List<CertificadoFundamental> Certificados { get; set; }
    }
}
