using System.Linq;
using Windows.ApplicationModel.Background;

namespace NFeFacil
{
    public static class RegistroClienteBackground
    {
        public static void Registrar()
        {
            var tasks = BackgroundTaskRegistration.AllTasks;
            var quant = tasks.Values.Count(x => x.Name == "BackgroundTaskCliente");
            if (quant != 1)
            {
                var socketTaskBuilder = new BackgroundTaskBuilder
                {
                    IsNetworkRequested = true,
                    Name = "BackgroundTaskCliente",
                    TaskEntryPoint = "Background.ClienteBackground"
                };
                socketTaskBuilder.SetTrigger(new TimeTrigger(15, false));
                socketTaskBuilder.Register();
            }
        }

        public static void Desrregistrar()
        {
            var tasks = BackgroundTaskRegistration.AllTasks;
            var quant = tasks.Values.Count(x => x.Name == "BackgroundTaskCliente");
            if (quant == 1)
            {
                tasks.Values.Single(x => x.Name == "BackgroundTaskCliente").Unregister(false);
            }
        }
    }
}
