namespace RestServer.Route
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

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
}
