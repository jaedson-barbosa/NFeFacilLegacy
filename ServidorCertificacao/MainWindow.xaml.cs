using System.Windows;
using ServidorCertificacao.Pacotes;
using System;
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
                string Cabecalho = string.Empty;
                string Corpo = string.Empty;
                var leitor = new StreamReader(stream);
                int tam = 0;
                while (true)
                {
                    var str = leitor.ReadLine();
                    Cabecalho += str + "\r\n";
                    if (str.Contains("Content-Length"))
                    {
                        tam = int.Parse(str.Substring(str.IndexOf(' ') + 1));
                    }
                    else if (string.IsNullOrEmpty(str))
                    {
                        if (tam == 0)
                        {
                            break;
                        }
                        else
                        {
                            tam -= 2;
                            char[] letras = new char[tam];
                            leitor.ReadBlock(letras, 0, tam);
                            Corpo = new string(letras);
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(Cabecalho))
                {
                    throw new Exception("Falha ao processar requisição.");
                }

                var primeiraLinha = Cabecalho.Substring(0, Cabecalho.IndexOf("\r\n"));
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
                        var xml0 = XElement.Parse(Corpo);
                        lstMetodos.Dispatcher.Invoke(() => lstMetodos.Items.Insert(0, $"XML Requisição: {xml0.ToString()};"), DispatcherPriority.Normal);
                        var cert = Desserializar<CertificadoAssinaturaDTO>(xml0);
                        Action<string> log = x => lstMetodos.Dispatcher.Invoke(() => lstMetodos.Items.Insert(0, x), DispatcherPriority.Normal);
                        texto = metodos.AssinarRemotamente(stream, cert, log);
                        break;
                    case "EnviarRequisicao":
                        var xml1 = XElement.Parse(Corpo);
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
