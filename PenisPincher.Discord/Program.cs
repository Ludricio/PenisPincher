using System;
using System.Threading.Tasks;

namespace PenisPincher.Discord
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await Bot.RunAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Uncaught exception: {e.Message}");
                throw;
            }
        }
    }
}
