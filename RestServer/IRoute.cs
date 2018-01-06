using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestServer
{
    public interface IRoute
    {
        Func<HttpListenerRequest, HttpListenerResponse, Task> HandlerAsync { get; set; }
        HttpMethod HttpMethod { get; set; }
        string Path { get; set; }

        bool Matches(string method, string path);
    }
}