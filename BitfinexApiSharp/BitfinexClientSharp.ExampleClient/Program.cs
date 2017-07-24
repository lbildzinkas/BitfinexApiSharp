using System;
using System.Threading;
using WebSocketSharp;

namespace BitfinexClientSharp.ExampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverUrl = "wss://api.bitfinex.com/ws";
            var ws = new WebSocket(serverUrl);

            ws.OnMessage += (sender, e) =>
            {
                Console.WriteLine("Laputa says: " + e.Data);
            };

            ws.OnOpen += (sender, args1) =>
            {
                ws.Send("{\r\n   \"event\":\"subscribe\",\r\n   \"channel\":\"ticker\"\r\n \"symbol\":\"BTCUSD\"\r\n}");
                Console.WriteLine("Laputa says: ");
            };

            ws.OnError += (sender, eventArgs) =>
            {
                Console.WriteLine("Laputa says: " + eventArgs.Message);
            };

            ws.Connect();

            ws.Send("{\r\n   \"event\":\"subscribe\",\r\n   \"channel\":\"ticker\"\r\n \"symbol\":\"BTCUSD\"\r\n}");

            Console.WriteLine("\nType 'exit' to exit.\n");
            while (true)
            {
                Thread.Sleep(1000);
                Console.Write("> ");
                //var msg = Console.ReadLine();
                //if (msg == "exit")
                //    break;

                // Send a text message.
                //ws.Send(msg);
            }
        }
    }
}