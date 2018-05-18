using System;

namespace ListBlobs
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
                return -1;

            Console.WriteLine(args.Length);

            return 0;
        }
    }
}
