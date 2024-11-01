using Microsoft.Extensions.Configuration;
using MultiplayerGame;
using System.IO;

class Program
{
    public static IConfiguration Configuration { get; private set; }

    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();
        int myPort = Configuration.GetValue<int>("ServerSettings:Port");
        string myIP = Configuration.GetValue<string>("ServerSettings:IpAddress");

        Server server;
        server = new Server(myPort, myIP);

        Console.ReadLine();
    }
}
