using Pipes.Azure;
using System;

namespace SendQueue
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
                return -1;

            string storageUrl = args[0];

            var queue = new StorageQueue(storageUrl);

            while (true)
            {
                string message = Console.ReadLine();

                if (message == null)
                    break;

                queue.Send(message);
            }

            return 0;
        }
    }
}
