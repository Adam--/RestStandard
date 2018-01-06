namespace RestServer.Route
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class DirectRoute : IRoute
    {
        public string Path { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public Func<HttpListenerRequest, HttpListenerResponse, Task> HandlerAsync { get; set; }

        public DirectRoute()
        {
        }

        public DirectRoute(
            HttpMethod method, 
            string path, 
            Func<HttpListenerRequest, HttpListenerResponse, Task> handlerAsync)
        {
            this.HttpMethod = method;
            this.Path = path;
            this.HandlerAsync = handlerAsync;
        }

        public bool Matches(string method, string path)
        {
            return this.HttpMethod.Method.Equals(method, StringComparison.OrdinalIgnoreCase) &&
                this.Path.Equals(path, StringComparison.OrdinalIgnoreCase);
        }
    }
}
