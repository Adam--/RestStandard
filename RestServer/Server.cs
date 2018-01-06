using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace RestServer
{
    public class Route : IRoute
    {
        public string Path { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public Func<HttpListenerRequest, HttpListenerResponse, Task> HandlerAsync { get; set; }
        public bool Matches(string method, string path)
        {
            return this.HttpMethod.Method.Equals(method, StringComparison.OrdinalIgnoreCase) &&
                this.Path.Equals(path, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class RegExRoute : IRoute
    {
        public string Path { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public Func<HttpListenerRequest, HttpListenerResponse, Task> HandlerAsync { get; set; }
        public bool Matches(string method, string path)
        {
            return this.HttpMethod.Method.Equals(method, StringComparison.OrdinalIgnoreCase) &&
                Regex.IsMatch(path, this.Path, RegexOptions.IgnoreCase);
        }
    }

    public class PredicateRoute : IRoute
    {
        Func<string, string, bool> matchPredicate;

        public PredicateRoute(Func<string, string, bool> matchPredicate)
        {
            this.matchPredicate = matchPredicate;
        }

        public string Path { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public Func<HttpListenerRequest, HttpListenerResponse, Task> HandlerAsync { get; set; }
        public bool Matches(string method, string path)
        {
            return matchPredicate(method, path);
        }
    }

    public class Server
    {
        public ICollection<IRoute> Routes { get; set; } = new Collection<IRoute>();

        private HttpListener Listener { get; set; }

        public void Listen(IPAddress address, int port)
        {
            this.Listener = new HttpListener(address, port);
            this.Listener.Request += Listener_Request;
            this.Listener.Start();
        }

        public void Stop()
        {
            this.Listener.Request -= Listener_Request;
            this.Listener.Dispose();
        }

        private async void Listener_Request(object sender, HttpListenerRequestEventArgs e)
        { 
            var route = this.Routes.FirstOrDefault(r => r.Matches(e.Request.HttpMethod, e.Request.Url.AbsolutePath));
            if (route == default(Route))
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
