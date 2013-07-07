using System;
using Microsoft.Owin.Hosting;

namespace Remote.Spotify
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:999"))
            {
                Console.WriteLine("Server running at http://localhost:999");
                Console.ReadLine();
            }
        }
    }
}











