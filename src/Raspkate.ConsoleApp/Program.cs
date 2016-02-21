using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Raspkate.Handlers;
using Raspkate.Config;
using System.Configuration;

namespace Raspkate.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //RaspkateServer server = new RaspkateServer("http://+:9023/", @"/home/pi/Projects");
            RaspkateServer server = new RaspkateServer(RaspkateConfiguration.Instance);
            server.Start();
            Console.WriteLine("Press ENTER to stop server.");
            Console.ReadLine();
            server.Stop();
        }
    }
}
