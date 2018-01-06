namespace RestServer
{
    using RestServer.Route;
    using System.Collections.Generic;
    using System.Net;

    public interface IServer
    {
        ICollection<IRoute> Routes { get; set; }
        bool IsListening { get; }
        void Listen(IPAddress address, int port);
        void Stop();
        IServer With(IRoute route);
    }
}