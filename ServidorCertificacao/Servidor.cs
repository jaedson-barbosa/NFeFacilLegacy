using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServidorCertificacao
{
    class Servidor
    {
        public event EventHandler<string> OnError;
        public event EventHandler<string> OnRequest;
        TcpListener listener;

        public async void Start()
        {
            var ip = Dns.GetHostEntry("localhost").AddressList[0];
            listener = new TcpListener(ip, 1010);
            listener.Start();
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                ProcessarRequisicao(client);
            }
        }

        void ProcessarRequisicao(TcpClient client)
        {
            var stream = client.GetStream();
            try
            {
                var leitor = new StreamReader(stream);
                var linha1 = leitor.ReadLine();
                var parametros = linha1
                    .Split(' ')[1]
                    .Substring(1)
                    .Split(new char[1] { '/' }, 2);
                string nomeMetodo = parametros[0],
                    caminhoXml = WebUtility.UrlDecode(parametros[1]);
                OnRequest(this, $"{nomeMetodo} \"{caminhoXml}\"");

                if (nomeMetodo == "Registrar")
                {
                    var rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    var path = AppDomain.CurrentDomain.BaseDirectory + "ServidorCertificacao.exe";
                    rkApp.SetValue("ConexaoA3", path);
                    EscreverCabecalho(0, true);
                }
                else
                {
                    string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    path = path.Substring(0, path.LastIndexOf('\\'));
                    Process process = new Process();
                    process.StartInfo.Domain = path;
                    process.StartInfo.WorkingDirectory = path;
                    process.StartInfo.FileName = "CertificacaoA3.exe";
                    process.StartInfo.Arguments = $"{nomeMetodo} \"{caminhoXml}\"";
                    process.Start();
                    process.WaitForExit();
                    var codigo = process.ExitCode;
                    process.Close();
                    EscreverCabecalho(0, codigo == 0);
                }
            }
            catch (Exception erro)
            {
                var mensagem = $"{erro.Message}\r\n" +
                    $"Local: {System.Reflection.Assembly.GetExecutingAssembly().Location}\r\n";
                OnError(this, mensagem);
                var data = Encoding.UTF8.GetBytes(mensagem);
                EscreverCabecalho(data.Length, false);
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                stream.Flush();
                stream.Dispose();
                client.Close();
                client.Dispose();
            }

            void EscreverCabecalho(int tamanho, bool sucesso)
            {
                var cabecalho = Encoding.UTF8.GetBytes($"HTTP/1.1 {(sucesso ? "200 OK" : "403 Forbidden")}\r\nContent-Length: {tamanho}\r\nConnection: close\r\n\r\n");
                stream.Write(cabecalho, 0, cabecalho.Length);
            }
        }

        public void Stop()
        {
            listener.Stop();
            listener = null;
        }
    }
}
