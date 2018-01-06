namespace RestServer.Route
{
    using System;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class RegExRoute : IRoute
    {
        public string Path { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public Func<HttpListenerRequest, HttpListenerResponse, Task> HandlerAsync { get; set; }

        public RegExRoute()
        {
        }

        public RegExRoute(
            HttpMethod method, 
            string regexPath, 
            Func<HttpListenerRequest, HttpListenerResponse, Task> handlerAsync)
        {
            this.HttpMethod = method;
            this.Path = regexPath;
            this.HandlerAsync = handlerAsync;
        }

        public bool Matches(string method, string path)
        {
            return this.HttpMethod.Method.Equals(method, StringComparison.OrdinalIgnoreCase) &&
                Regex.IsMatch(path, this.Path, RegexOptions.IgnoreCase);
        }
    }
}
