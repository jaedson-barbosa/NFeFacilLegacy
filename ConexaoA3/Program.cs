using System;
using System.Management;
using Topshelf;

namespace ConexaoA3
{
    class Program
    {
        const string ServiceName = "ConexaoA3";
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "configure")
            {
                string userName = null, password = null;
                if (args.Length != 5)
                    Console.WriteLine("Parâmetros inválidos.");
                else
                {
                    if (args[1] == "-u")
                    {
                        userName = args[2];
                        if (args[3] == "-p")
                            password = args[4];
                        else
                            Console.WriteLine("-p era esperado.");
                    }
                    else if (args[1] == "-p")
                    {
                        password = args[2];
                        if (args[3] == "-u")
                            userName = args[4];
                        else
                            Console.WriteLine("-u era esperado.");
                    }
                }

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    string objPath = string.Format("Win32_Service.Name='{0}'", ServiceName);
                    using (var service = new ManagementObject(new ManagementPath(objPath)))
                    {
                        object[] wmiParams = new object[11];
                        wmiParams[6] = @".\" + userName;
                        wmiParams[7] = password;
                        var result = service.InvokeMethod("Change", wmiParams);
                    }
                }
                else
                    Console.WriteLine("Faltam informações! Por favor, tenha certeza que tanto o usuário quanto a senha foram informados.\nDesinstale o serviço e tente novamente.");
            }
            else
            {
                HostFactory.Run(configure =>
                {
                    configure.Service<Servidor>(service =>
                    {
                        service.ConstructUsing(s => new Servidor());
                        service.WhenStarted(s => s.Start());
                        service.WhenStopped(s => s.Stop());
                    });
                    configure.RunAsLocalSystem()
                        .StartAutomatically();
                    configure.SetServiceName(ServiceName);
                    configure.SetDisplayName("Conexão A3");
                    configure.SetDescription("Conexão entre NFe Fácil e certificador do Usuário.");
                });
            }
        }
    }
}
