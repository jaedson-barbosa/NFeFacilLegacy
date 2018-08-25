using ConexaoA3.Pacotes;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ConexaoA3
{
    class Servidor
    {
        public async void Start()
        {
            var ip = Dns.GetHostEntry("localhost").AddressList[0];
            var listener = new TcpListener(ip, 1010);

            listener.Start();
            Console.WriteLine("Iniciado servidor.");
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                ProcessarRequisicao(client);
            }
        }

        async void ProcessarRequisicao(TcpClient client)
        {
            Console.WriteLine("Requisição recebida.");
            var stream = client.GetStream();
            var metodos = new Metodos();
            try
            {
                var leitor = new StreamReader(stream);
                var linha1 = leitor.ReadLine();
                Console.WriteLine($"Linha original: {linha1}");
                var parametros = linha1.Split(' ')[1].Substring(1).Split(new char[1] { '/' }, 2);
                Console.WriteLine($"Nome metodo: {parametros[0]}.");
                string texto = null;
                switch (parametros[0])
                {
                    case "ObterCertificados":
                        texto = metodos.ObterCertificados(stream);
                        break;
                    case "AssinarRemotamente":
                        var xml0 = XElement.Load(parametros[1]);
                        var cert = Desserializar<CertificadoAssinaturaDTO>(xml0);
                        texto = metodos.AssinarRemotamente(stream, cert);
                        break;
                    case "EnviarRequisicao":
                        var xml1 = XElement.Load(parametros[1]);
                        var envio = Desserializar<RequisicaoEnvioDTO>(xml1);
                        texto = await metodos.EnviarRequisicaoAsync(stream, envio);
                        break;
                    default:
                        texto = "Método não reconhecido.";
                        break;
                }
                var data = Encoding.UTF8.GetBytes(texto);
                EscreverCabecalho(data.Length);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception erro)
            {
                var mensagem = $"{erro.Message}\r\n" +
                    $"Detalhes adicionais: {erro.InnerException?.Message}";
                Console.WriteLine($"Erro: {mensagem}.");
                var data = Encoding.UTF8.GetBytes(mensagem);
                EscreverCabecalho(data.Length);
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                await Task.Delay(200);
                stream.Flush();
                stream.Dispose();
                client.Close();
                client.Dispose();
            }

            void EscreverCabecalho(int tamanho)
            {
                var cabecalho = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\r\nContent-Length: {tamanho}\r\nConnection: close\r\n\r\n");
                stream.Write(cabecalho, 0, cabecalho.Length);
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

        public void Stop()
        {

        }
    }
}
