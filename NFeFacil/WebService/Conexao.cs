using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using static NFeFacil.Configuracoes.ConfiguracoesCertificacao;

namespace NFeFacil.WebService
{
    internal sealed class Conexao<T> : IDisposable
    {
        private ChannelFactory<T> CanalComunicação;

        internal Conexao(string endereco)
        {
            var bind = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            bind.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            CanalComunicação = new ChannelFactory<T>(bind, new EndpointAddress(endereco));
            CanalComunicação.Credentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySerialNumber, Certificado);
        }

        public void Dispose()
        {
            ((IDisposable)CanalComunicação).Dispose();
            GC.SuppressFinalize(this);
        }

        internal T EstabelecerConexão() => CanalComunicação.CreateChannel();
    }
}
