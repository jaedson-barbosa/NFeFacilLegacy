using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace NFeFacil.Sincronizacao.FastServer
{
    public class HttpServer : IDisposable
    {
        private readonly int _port;
        private StreamSocketListener _listener;
        readonly RestControllerRequestHandler _requestHandler;

        public HttpServer(int porta)
        {
            _port = porta;
            _listener = new StreamSocketListener();

            _listener.ConnectionReceived += ProcessRequestAsync;
            _requestHandler = new RestControllerRequestHandler();
        }

        public void RegisterController<T>() where T : class, new()
        {
            _requestHandler.RegisterController<T>();
        }

        public async Task StartServerAsync()
        {
            await _listener.BindServiceNameAsync(_port.ToString());
        }

        public void StopServer()
        {
            ((IDisposable)this).Dispose();
        }

        async void ProcessRequestAsync(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            using (var socket = args.Socket)
            {
                string httpResponse;
                try
                {
                    using (var input = socket.InputStream)
                    {
                        var buffer = new Windows.Storage.Streams.Buffer(10);
                        var result = await input.ReadAsync(buffer, 10, InputStreamOptions.None);
                        var tamanho = uint.Parse(Encoding.UTF8.GetString(result.ToArray()));

                        buffer = new Windows.Storage.Streams.Buffer(tamanho);
                        result = await input.ReadAsync(buffer, tamanho, InputStreamOptions.None);
                        var xml = XElement.Load(result.AsStream());

                        var request = new RestRequest
                        {
                            Content = xml.Element("Content"),
                            Uri = xml.Element("Uri").Value
                        };
                        httpResponse = await _requestHandler.HandleRequestAsync(request);
                    }
                }
                catch (Exception e)
                {
                    httpResponse = e.Message;
                }

                httpResponse = $"{httpResponse.Length.ToString("0000000000")}{httpResponse}";
                using (var output = socket.OutputStream)
                {
                    await output.WriteAsync(Encoding.UTF8.GetBytes(httpResponse).AsBuffer());
                    await output.FlushAsync();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_listener != null)
                {
                    _listener.Dispose();
                    _listener = null;
                }
            }
        }
    }
}
