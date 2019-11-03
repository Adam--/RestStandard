# RestStandard

This is a basic, non-compliant, POC rest server built with netstandard 1.4. I built this  when I had a few free hours during vacation around Dec 2017 just for fun. This is not intended to be feature complete or production worthy. As I write this readme and publish the repo years after I originally wrote the code, I realize that a lot has changed in the Xamarin and the netstandard world since Dec 2017 and there are many improvements that could easily be made.

Up until netstandard 2.0, System.Net did not include an HttpListener class. RestStandard uses the [NETStandard.HttpListener](https://github.com/StefH/NETStandard.HttpListener) library by [Stef Heyenrath](https://github.com/StefH) licensed under the [MIT License](https://github.com/StefH/NETStandard.HttpListener/blob/master/LICENSE).

## Configuring the server and routes

Create a new server

```cs
var server = new Server()
    .With(new DirectRoute(
            HttpMethod.Get,
            "/",
            async (req, resp) =>
            {
                await resp.WriteContentAsync($"hello {DateTime.Now}");
            }));
```

Routes are added in a fluent manner calling the .With function on the server. If there are multiple matching routes, the first match found will be used.

### Route types

There are 3 route types `DirectRoute`, `PredicateRoute`, and `RegExRoute`. You can create your own route by implementing IRoute.

`DirectRoute` is supplied with a direct path that is used when determining if a request's path matches. Casing is ignored. A match is made when the route's path and the request's path match fully, ignoring case.

`PredicateRoute` is supplied with a predicate function that is called when evaluating if a request's path matches. The predicate is defined as `Func<string, string, bool>`.

`RegExRoute` is supplied with a regular expression that the request's route is matched on.

> Note: If there are multiple matching routes, the first match found will be used.

## Running on Windows

Listen on all network adapters on a specified port

```cs
server.Listen(IPAddress.Any, 8081);
```

## Running on Android

Determine what IP address to listen on

```cs
WifiManager manager = (WifiManager)GetSystemService(Service.WifiService);
var ipAddress = new IPAddress(manager.ConnectionInfo.IpAddress);
```

Start listening on that IP address on a specified port

```cs
server.Listen(ipAddress, 8081);
```
