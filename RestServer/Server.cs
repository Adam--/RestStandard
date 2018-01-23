namespace RestServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net;
    using System.Collections.ObjectModel;
    using Route;
    using System.Threading.Tasks;

    public class Server : IServer
    {
        public bool IsListening => this.Listener?.IsListening ?? false;

        private HttpListener Listener { get; set; }

        private IRouter Router { get; set; } = new Router();

        public void Listen(IPAddress address, int port)
        {
            if (this.IsListening)
            {
                throw new InvalidOperationException("Server is already listening");
            }

            this.Listener = new HttpListener(address, port);
            this.Listener.Request += Listener_Request;
            this.Listener.Start();
        }

        public void Stop()
        {
            if (!this.IsListening)
            {
                throw new InvalidOperationException("Server is not listening, unable to stop");
            }

            this.Listener.Request -= Listener_Request;
            this.Listener.Dispose();
        }

        public static IServer Create()
        {
            return new Server();
        }

        public IServer With(IRoute route)
        {
            this.Router.Add(route);
            return this;
        }

        private async void Listener_Request(object sender, HttpListenerRequestEventArgs e)
        {
            await this.Router.HandleAsync(e.Request, e.Response);
        }
    }
}
