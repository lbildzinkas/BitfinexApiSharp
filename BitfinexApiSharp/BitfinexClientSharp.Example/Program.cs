using System;
using System.Text;
using System.Threading;
using BitfinexClientSharp.Dtos;
using BitfinexClientSharp.WSocket;

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

            var client = new WSocketClient("wss://api.bitfinex.com/ws/2", delay);

            client.TickerSubscribe(Pair.BTCUSD, LogStatus1);
            client.BookSubscribe(Pair.BTCUSD, LogStatus1);
            client.TradeSubscribe(Pair.BTCUSD, LogStatus1);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void LogStatus1(byte[] buffer)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor =  ConsoleColor.Green;

                Console.WriteLine(encoder.GetString(buffer));

                Console.ResetColor();
            }
        }
    }
}
