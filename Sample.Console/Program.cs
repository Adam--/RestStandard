
using RestServer;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            var server = new Server();
            server.Routes.Add(
                new Route
                {
                    HttpMethod = new HttpMethod(HttpMethods.Get),
                    Path = "/",
                    HandlerAsync = async (req, resp) =>
                    {
                        await resp.WriteContentAsync($"hello at {DateTime.Now}");
                    }
                });
            server.Routes.Add(
                new Route
                {
                    HttpMethod = new HttpMethod(HttpMethods.Post),
                    Path = "/echo",
                    HandlerAsync = async (req, resp) =>
                    {
                        var content = await req.ReadContentAsStringAsync();
                        await resp.WriteContentAsync(content);
                    }
                });
            //server.AddPost("/echo", async (req, resp) => 
            //{
            //    var content = await req.ReadContentAsStringAsync();
            //    await resp.WriteContentAsync(content);
            //});
            server.Listen(IPAddress.Parse("127.0.0.1"), 8081);
            Console.WriteLine("Listening on 127.0.0.1:8081");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
