using System;

namespace ServidorCertificacaoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Iniciar();
            Console.Read();
        }

        async static void Iniciar()
        {
            var helper = new TcpHelper();
            var ip = await helper.StartServer();
            helper.Listen();

            Console.WriteLine("Servidor iniciado");
            Console.WriteLine($"IP: {ip}");
            Console.WriteLine($"Porta: 8080");

            Console.Write("Para parar pressione qualquer tecla.");
        }
    }
}