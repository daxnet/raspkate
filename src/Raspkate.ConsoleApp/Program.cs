using System;
using System.Collections.Generic;
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
            Console.WriteLine("Raspkate - A simple web server{0}", Environment.NewLine);

            RaspkateServer server = new RaspkateServer(RaspkateConfiguration.Instance);
            server.Start();
            Console.WriteLine("{0}Press ENTER to stop the server.", Environment.NewLine);
            Console.ReadLine();
            server.Stop();
            Console.WriteLine("{0}Raspkate service stopped.{0}", Environment.NewLine, Environment.NewLine);
        }
    }
}
