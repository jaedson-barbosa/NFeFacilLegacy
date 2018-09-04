using CertificacaoA3.Pacotes;
using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CertificacaoA3
{
    class Program
    {
        static int Main(string[] args)
        {
            var metodos = new Metodos();
            try
            {
                XElement salvar = null;
                switch (args[0])
                {
                    case "ObterCertificados":
                        salvar = metodos.ObterCertificados();
                        break;
                    case "AssinarRemotamente":
                        var xml0 = XElement.Load(args[1]);
                        var cert = Desserializar<CertificadoAssinaturaDTO>(xml0);
                        salvar = metodos.AssinarRemotamente(cert);
                        break;
                    case "EnviarRequisicao":
                        var xml1 = XElement.Load(args[1]);
                        var envio = Desserializar<RequisicaoEnvioDTO>(xml1);
                        salvar = metodos.EnviarRequisicaoAsync(envio).Result;
                        break;
                }
                salvar.Save(args[1]);
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        static T Desserializar<T>(XElement xml)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = xml.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }
    }
}
