using System;
using System.Text;
using System.Threading;
using BitfinexClientSharp.Dtos;
using BitfinexClientSharp.WSocket;
using BitfinexClientSharp.WSocket.Adapters;

namespace BitfinexClientSharp.Example
{
    class Program
    {
        private static object consoleLock = new object();
        private static readonly TimeSpan delay = TimeSpan.FromMilliseconds(30000);
        private static UTF8Encoding encoder = new UTF8Encoding();

        static void Main(string[] args)
        {
            Thread.Sleep(1000);

            var client = new WSocketClient(new WSocketResponseAdapterFactory(new ResponseValidator()),  "wss://api.bitfinex.com/ws/2", delay);

            client.TickerSubscribe(Pair.BTCUSD, LogStatusTicker);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void LogStatusTicker(IResponse obj)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                var ticker = (TickerResponse) obj;
                Console.WriteLine($"{ticker?.Pair} - {ticker?.LastPrice}");

                Console.ResetColor();
            }
        }

    }
}
