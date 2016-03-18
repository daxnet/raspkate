using System;
using System.Collections.Generic;
using Raspkate.Handlers;
using Raspkate.Config;
using System.Configuration;

namespace Raspkate.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            RaspkateServer server = new RaspkateServer(RaspkateConfiguration.Instance);
            server.Start();
            Console.ReadLine();
            server.Stop();
        }
    }
}
