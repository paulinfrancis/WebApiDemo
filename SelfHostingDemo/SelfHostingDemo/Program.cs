using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace SelfHostingDemo
{
    class Program
    {
        private static HttpSelfHostServer _selfHostServerServer;

        static void Main(string[] args)
        {
            var port = ConfigurationManager.AppSettings["Port"] ?? "8080";
            var hostname = ConfigurationManager.AppSettings["Hostname"] ?? "localhost";
            
            //The url that the service can be called on
            var serviceUrl = string.Format("http://{0}:{1}", hostname, port);

            //Service configuration
            var config = new HttpSelfHostConfiguration(serviceUrl);

            //Add routes to controllers
            config.Routes.MapHttpRoute("DefaultApi ", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            //Writes list of available mediatype formatters to console
            Console.WriteLine("{0}{1}", "Registered media type formatters:\n", string.Join("\n", config.Formatters.Select(x => x.GetType())));
       
            //Initialize server
            _selfHostServerServer = new HttpSelfHostServer(config);

            //Start server
            _selfHostServerServer.OpenAsync().Wait();
            Console.WriteLine("{1}Self hosting server is running on: {0}", serviceUrl, Environment.NewLine);
            Console.WriteLine("Press Enter to quit.");
            Console.ReadLine();

            //Wires up application close handler
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
                                                       {
                                                           //Close server
                                                           _selfHostServerServer.CloseAsync();
                                                           _selfHostServerServer.Dispose();
                                                       };
        }
    }
}
