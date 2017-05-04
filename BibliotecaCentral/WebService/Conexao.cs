using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace BibliotecaCentral.WebService
{
    internal struct Conexao<T> : IDisposable
    {
        private ChannelFactory<T> CanalComunicação;

        internal Conexao(string endereco)
        {
            var bind = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            bind.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            CanalComunicação = new ChannelFactory<T>(bind, new EndpointAddress(endereco));
            var repo = new Repositorio.Certificados();
            CanalComunicação.Credentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySerialNumber, repo.SerialEscolhido);
        }

        public void Dispose()
        {
            ((IDisposable)CanalComunicação).Dispose();
            GC.SuppressFinalize(this);
        }

        internal T EstabelecerConexão() => CanalComunicação.CreateChannel();
    }
}
