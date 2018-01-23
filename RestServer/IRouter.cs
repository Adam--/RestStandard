namespace RestServer
{
    using Route;
    using System.Net.Http;
    using System.Threading.Tasks;

    interface IRouter
    {
        void Add(IRoute route);
        Task HandleAsync(HttpListenerRequest request, HttpListenerResponse response);
    }
}
