using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ArmAPI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var host = new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseKestrel()
                    .UseStartup<Startup>()
                    .UseUrls("http://+:88")

                    .Build();

                host.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}