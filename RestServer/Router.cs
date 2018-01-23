using RestServer.Route;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace RestServer
{
    class Router : IRouter
    {
        public ICollection<IRoute> Routes { get; set; } = new Collection<IRoute>();

        public void Add(IRoute route)
        {
            this.Routes.Add(route);
        }

        public async Task HandleAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var route = this.Routes.FirstOrDefault(r => r.Matches(request.HttpMethod, request.Url.AbsolutePath));
            if (route == default(IRoute))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();
                return;
            }
            await route.HandlerAsync.Invoke(request, response);
            response.Close();
        }
    }
}
