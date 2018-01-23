namespace RestServer
{
    using RestServer.Route;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IServer
    {
        bool IsListening { get; }
        void Listen(IPAddress address, int port);
        void Stop();
        IServer With(IRoute route);
    }

    //public interface IServerMethods
    //{
    //    void Get(string route, Func<HttpListenerRequest, HttpListenerResponse, Task> handlerAsync);
    //    void Post(string route, Func<HttpListenerRequest, HttpListenerResponse, Task> handlerAsync);

    //}
}