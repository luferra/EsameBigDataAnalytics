using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dcn
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer httpServer = new HttpServer(5051);
            WebSocket webSocket = new WebSocket(httpServer, 5053);

            Console.WriteLine("DCN web service.");
            Console.WriteLine("Press 'q' to quit");

            while (Console.ReadLine() != "q") ;

            httpServer.Close();
            webSocket.Close();
        }
    }
}
