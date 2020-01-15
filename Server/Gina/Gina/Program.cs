using System;
using System.Threading;

namespace Gina
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            var server = new AsynchronousSocketListener();
            var server_therad = new Thread(new ThreadStart(server.StartListening));
            server_therad.Start();
            Console.Read();
            server.on = false;
        }
    }
}
