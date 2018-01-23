
using RestServer;
using RestServer.Route;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            Server.Create()
                  .With(
                      new DirectRoute(
                          HttpMethod.Get,
                          "/",
                          async (req, resp) =>
                          {
                              await resp.WriteContentAsync($"hello at {DateTime.Now}");
                          }))
                  .With(
                      new DirectRoute(
                          HttpMethod.Post,
                          "/echo",
                          async (req, resp) =>
                          {
                              var content = await req.ReadContentAsStringAsync();
                              //await resp.WriteContentAsync(content);
                              var parameters = req.Url.ParseQueryParameters();
                              var parameterJson = JsonConvert.SerializeObject(parameters);
                              await resp.WriteContentAsync(content + parameterJson);
                          }
                          ))
                  .Listen(IPAddress.Any, 8081);

            //var server = new Server();
            //server.Routes.Add(
            //    new Route
            //    {
            //        HttpMethod = HttpMethod.Get,
            //        Path = "/",
            //        HandlerAsync = async (req, resp) =>
            //        {
            //            await resp.WriteContentAsync($"hello at {DateTime.Now}");
            //        }
            //    });
            //server.Routes.Add(
            //    new Route
            //    {
            //        HttpMethod = HttpMethod.Post,
            //        Path = "/echo",
            //        HandlerAsync = async (req, resp) =>
            //        {
            //            var content = await req.ReadContentAsStringAsync();
            //            await resp.WriteContentAsync(content);
            //        }
            //    });
            //server.Listen(IPAddress.Parse("127.0.0.1"), 8081);

            Console.WriteLine("Listening");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
