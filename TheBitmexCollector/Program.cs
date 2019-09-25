using System;
using System.Threading;
using System.Threading.Tasks;

namespace TheBitmexCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TheCollector collector = new TheCollector();
                collector.Start();
                var endlessTask = new TaskCompletionSource<bool>().Task;
                endlessTask.Wait();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Thread.Sleep(60000);
                Main(null);
            }
            
        }
    }
}
