using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BitfinexClientSharp.WSocket;

namespace BitfinexClientSharp.Example
{
    class Program
    {
        private static object consoleLock = new object();
        private const int receiveChunkSize = 256;
        private const bool verbose = true;
        private static readonly TimeSpan delay = TimeSpan.FromMilliseconds(30000);
        private static UTF8Encoding encoder = new UTF8Encoding();

        static void Main(string[] args)
        {
            Thread.Sleep(1000);
            var client = new WSocketClient("wss://api.bitfinex.com/ws/2", delay);
            client.TickerSubscribe(CryptoPair.BTCUSD, LogStatus1);

            client.TickerSubscribe(CryptoPair.ETHUSD, LogStatus1);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void LogStatus1(byte[] buffer)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor =  ConsoleColor.Green;

                if (verbose)
                    Console.WriteLine(encoder.GetString(buffer));

                Console.ResetColor();
            }
        }
    }
}
