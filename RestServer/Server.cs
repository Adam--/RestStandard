namespace RestServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net;
    using System.Collections.ObjectModel;
    using RestServer.Route;

    public class Server : IServer
    {
        public ICollection<IRoute> Routes { get; set; } = new Collection<IRoute>();

        public bool IsListening => this.Listener?.IsListening ?? false;

        private HttpListener Listener { get; set; }

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
            this.Routes.Add(route);
            return this;
        }

        private async void Listener_Request(object sender, HttpListenerRequestEventArgs e)
        { 
            var route = this.Routes.FirstOrDefault(r => r.Matches(e.Request.HttpMethod, e.Request.Url.AbsolutePath));
            if (route == default(IRoute))
            {
                e.Response.StatusCode = (int)HttpStatusCode.NotFound;
                e.Response.Close();
                return;
            }
            await route.HandlerAsync.Invoke(e.Request, e.Response);
            e.Response.Close();
        }
    }
}
