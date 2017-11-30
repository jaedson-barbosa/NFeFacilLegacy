using System.Windows;
using Comum.Pacotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.IO;

namespace ServidorCertificacao
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lstMetodos.Dispatcher.Invoke(() => lstMetodos.Items.Insert(0, "A porta usada é a 1010"), DispatcherPriority.Normal);
            Listener0();
            Listener1();
        }

        async void Listener0()
        {
            var lista = (await Dns.GetHostAddressesAsync(Dns.GetHostName()));
            var ip = lista.First(x => x.AddressFamily == AddressFamily.InterNetwork);

            var listener = new TcpListener(ip, 1010);

            listener.Start();
            lstMetodos.Dispatcher.Invoke(() => lstMetodos.Items.Insert(0, $"Iniciado servidor em: {ip.MapToIPv4().ToString()};"), DispatcherPriority.Normal);
            if (listener != null)
            {
                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    ThreadPool.QueueUserWorkItem(ProcessarRequisicao, client);
                }
            }
        }

        async void Listener1()
        {
            var ip = Dns.GetHostEntry("localhost").AddressList[0];
            var listener = new TcpListener(ip, 1010);

            listener.Start();
            lstMetodos.Dispatcher.Invoke(() => lstMetodos.Items.Insert(0, "Iniciado servidor em: localhost;"), DispatcherPriority.Normal);
            if (listener != null)
            {
                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    ThreadPool.QueueUserWorkItem(ProcessarRequisicao, client);
                }
            }
        }

        async void ProcessarRequisicao(object obj)
        {
            lstMetodos.Dispatcher.Invoke(() => lstMetodos.Items.Insert(0, "Requisição recebida."), DispatcherPriority.Normal);
            var client = (TcpClient)obj;
            var stream = client.GetStream();
            var metodos = new Metodos();
            try
            {
                var bytes = new List<byte>();
                while (stream.DataAvailable)
                {
                    bytes.Add((byte)stream.ReadByte());
                }
                var requisicao = Encoding.UTF8.GetString(bytes.ToArray());

                var primeiraLinha = requisicao.Substring(0, requisicao.IndexOf("\r\n"));
                var parametros = primeiraLinha.Split(' ')[1].Replace('/', ' ').Trim().Split(' ');
                var nomeMetodo = parametros[0];
                lstMetodos.Dispatcher.Invoke(() => lstMetodos.Items.Insert(0, $"Nome metodo: {nomeMetodo};"), DispatcherPriority.Normal);
                string texto;
                switch (nomeMetodo)
                {
                    case "ObterCertificados":
                        texto = metodos.ObterCertificados(stream);
                        break;
                    case "AssinarRemotamente":
                        var conteudo0 = requisicao.Substring(requisicao.IndexOf("\r\n\r\n") + 4);
                        var xml0 = XElement.Parse(conteudo0);
                        var cert = Desserializar<CertificadoAssinaturaDTO>(xml0);

                        texto = metodos.AssinarRemotamente(stream, cert);
                        break;
                    case "EnviarRequisicao":
                        var conteudo1 = requisicao.Substring(requisicao.IndexOf("\r\n\r\n") + 4);
                        var xml1 = XElement.Parse(conteudo1);
                        var envio = Desserializar<RequisicaoEnvioDTO>(xml1);

                        texto = await metodos.EnviarRequisicaoAsync(stream, envio);
                        break;
                    default:
                        texto = "Método não reconhecido.";
                        break;
                }
                var data = Encoding.UTF8.GetBytes(texto);
                EscreverCabecalho(stream, data.Length);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception erro)
            {
                var mensagem = $"{erro.Message}\r\n" +
                    $"Detalhes adicionais: {erro.InnerException?.Message}";
                lstMetodos.Dispatcher.Invoke(() => lstMetodos.Items.Insert(0, $"Erro: {mensagem};"), DispatcherPriority.Normal);
                var data = Encoding.UTF8.GetBytes(mensagem);
                EscreverCabecalho(stream, data.Length);
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
        }

        static T Desserializar<T>(XElement xml)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = xml.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        static void EscreverCabecalho(Stream stream, int tamanho)
        {
            var cabecalho = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\r\nContent-Length: {tamanho}\r\nConnection: close\r\n\r\n");
            stream.Write(cabecalho, 0, cabecalho.Length);
        }
    }
}
