using System;
using Pipes.Azure;

namespace ReadQueue
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
                return -1;

            string storageUrl = args[0];

            var queue = new StorageQueue(storageUrl);
            
            while(true)
            {
                var message = queue.Read();
                Console.WriteLine(message);
            }
        }
    }
}
