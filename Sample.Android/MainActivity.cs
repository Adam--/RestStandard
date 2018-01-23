using Android.App;
using Android.Widget;
using Android.OS;
using RestServer;
using System.Net;
using System;
using Android.Net.Wifi;
using RestServer.Route;
using System.Net.Http;

namespace Sample.Android
{
    [Activity(Label = "Sample.Android", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var server = new Server()
                .With(new DirectRoute(
                        HttpMethod.Get,
                        "/",
                        async (req, resp) =>
                        {
                            await resp.WriteContentAsync($"hello at {DateTime.Now}");
                        }));
            //server.AddGet("/", async (req, resp) =>
            //{
            //    await resp.WriteContentAsync($"hello at {DateTime.Now}");
            //});
            //server.AddPost("/echo", async (req, resp) =>
            //{
            //    var content = await req.ReadContentAsStringAsync();
            //    await resp.WriteContentAsync(content);
            //});

            WifiManager manager = (WifiManager)GetSystemService(Service.WifiService);
            var ipAddress = new IPAddress(manager.ConnectionInfo.IpAddress);
            Console.WriteLine($"Listening on {ipAddress}:8081");
            server.Listen(ipAddress, 8081);
        }
    }
}

