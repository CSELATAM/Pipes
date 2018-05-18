using Pipes.Azure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ListBlobs
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
                return -1;

            if (args.Length == 1)
                return -1;

            //var blob = (args.Length == 1) ? new BlobFolder(args[0]) : new BlobFolder(args[0], args[1]);

            string storageUrl = args[0];
            //string filePattern = args[1];

            var blobFolder = new BlobFolder(storageUrl);

            foreach(var filename in ListFiles(blobFolder, "2010/*.pdf"))
            {
                Console.WriteLine(filename);
            }

            return 0;
        }

        static IEnumerable<string> ListFiles(BlobFolder blob, string pattern)
        {
            string prefix = "";
            string suffix = "";

            if (pattern != null)
            {
                var filePatternComponents = pattern.Split("*");
                prefix = filePatternComponents[0];

                suffix = (filePatternComponents.Length == 2) ? filePatternComponents[1] : "";
            }

            var result = blob
                            .EnumItems(prefix)
                            .Where(name => name.EndsWith(suffix));

            return result;
        }
    }
}
