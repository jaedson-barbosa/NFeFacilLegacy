using Comum.Pacotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ServidorCertificacaoConsole
{
    class TcpHelper
    {
        private TcpListener Listener { get; set; }
        private bool Accept { get; set; } = false;

        public async Task<string> StartServer()
        {
            var lista = (await Dns.GetHostAddressesAsync(Dns.GetHostName()));
            var ip = lista.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            Listener = new TcpListener(ip, 8080);

            Listener.Start();
            Accept = true;
            return ip.ToString();
        }

        public async void Listen()
        {
            if (Listener != null && Accept)
            {
                while (true)
                {
                    var client = await Listener.AcceptTcpClientAsync();
                    ThreadPool.QueueUserWorkItem(ProcessarRequisicao, client);
                }
            }
        }

        async static void ProcessarRequisicao(object obj)
        {
            var client = (TcpClient)obj;
            var stream = client.GetStream();
            var metodos = new Metodos();
            try
            {
                var requisicao = ReadToEnd(stream);

                var primeiraLinha = requisicao.Substring(0, requisicao.IndexOf("\r\n"));
                var parametros = primeiraLinha.Split(' ')[1].Replace('/', ' ').Trim().Split(' ');
                var nomeMetodo = parametros[0];
                Console.WriteLine($"Nome metodo: {nomeMetodo};");
                switch (nomeMetodo)
                {
                    case Comum.NomesMetodos.ObterCertificados:
                        metodos.ObterCertificados(stream);
                        break;
                    case Comum.NomesMetodos.ObterChaveCertificado:
                        var parametroMetodo = parametros.Length == 1 ? null : WebUtility.UrlDecode(parametros[1]);
                        Console.WriteLine($"Parametro: {parametroMetodo}");
                        metodos.ObterChaveCertificado(stream, parametroMetodo);
                        break;
                    case Comum.NomesMetodos.EnviarRequisicao:
                        var conteudo = requisicao.Substring(requisicao.IndexOf("\r\n\r\n") + 4);
                        var xml = XElement.Parse(conteudo);
                        var envio = Desserializar<RequisicaoEnvioDTO>(xml);
                        await metodos.EnviarRequisicaoAsync(stream, envio);
                        break;
                    default:
                        throw new Exception("Método não reconhecido.");
                }
            }
            catch (Exception e0)
            {
                var data = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\r\nContent-Length: {e0.Message.Length}\r\nConnection: close\r\n\r\n{e0.Message}");
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                stream.Flush();
                stream.Dispose();
                client.Close();
                client.Dispose();
            }
        }

        static string ReadToEnd(NetworkStream stream)
        {
            var bytes = new List<byte>();
            while (stream.DataAvailable)
            {
                bytes.Add((byte)stream.ReadByte());
            }
            return Encoding.UTF8.GetString(bytes.ToArray());
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