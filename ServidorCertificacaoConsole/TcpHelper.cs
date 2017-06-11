using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
                var metodos = new Metodos();
                while (true)
                {
                    using (var client = await Listener.AcceptTcpClientAsync())
                    {
                        var stream = client.GetStream();
                        using (StreamReader leitor = new StreamReader(stream))
                        {
                            try
                            {
                                var parametro = leitor.ReadLine();
                                var parametros = parametro.Split(' ')[1].Replace('/', ' ').Trim().Split(' ');

                                var nomeMetodo = parametros[0];
                                var parametroMetodo = parametros.Length == 1 ? null : WebUtility.UrlDecode(parametros[1]);

                                Console.WriteLine($"Nome metodo: {nomeMetodo}; parametro: {parametroMetodo}");

                                switch (nomeMetodo)
                                {
                                    case "ObterCertificados":
                                        metodos.ObterCertificados(stream);
                                        break;
                                    case "ObterCertificado":
                                        if (parametroMetodo != null)
                                        {
                                            metodos.ObterCertificado(stream, parametroMetodo);
                                            break;
                                        }
                                        else
                                        {
                                            throw new Exception();
                                        }
                                    default:
                                        throw new Exception();
                                }
                            }
                            catch (Exception e0)
                            {
                                try
                                {
                                    var data = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\r\nContent-Length: {e0.Message.Length}\r\nConnection: close\r\n\r\n{e0.Message}");
                                    stream.Write(data, 0, data.Length);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            stream.Flush();
                        }
                    }
                }
            }
        }
    }
}