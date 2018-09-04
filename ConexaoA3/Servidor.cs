using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

        void ProcessarRequisicao(TcpClient client)
        {
            Console.WriteLine("Requisição recebida.");
            var stream = client.GetStream();
            try
            {
                var leitor = new StreamReader(stream);
                var linha1 = leitor.ReadLine();
                var parametros = linha1
                    .Split(' ')[1]
                    .Substring(1)
                    .Split(new char[1] { '/' }, 2);
                var nomeMetodo = parametros[0];
                var caminhoXml = parametros[1];
                const string caminhoExe = "CertificacaoA3.exe";

                Console.WriteLine($"Nome metodo: {nomeMetodo}");
                Console.WriteLine($"Caminho Xml: {caminhoXml}");

                Process process = new Process();
                process.StartInfo.FileName = caminhoExe;
                process.StartInfo.Arguments = $"{nomeMetodo} \"{caminhoXml}\"";
                process.Start();
                process.WaitForExit();
                var codigo = process.ExitCode;
                process.Close();
                EscreverCabecalho(0, codigo == 0);
            }
            catch (Exception erro)
            {
                var mensagem = $"{erro.Message}\r\n" +
                    $"Detalhes adicionais: {erro.InnerException?.Message}";
                Console.WriteLine($"Erro: {mensagem}.");
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

        }
    }
}
