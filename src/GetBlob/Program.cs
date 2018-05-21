using Pipes.Azure;
using System;
using System.IO;

namespace GetBlob
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
                return -1;

            string storageUrl = args[0];

            var blobFolder = new BlobFolder(storageUrl);

            Loop(blobFolder);

            return 0;
        }

        static void Loop(BlobFolder blobFolder)
        {
            while(true)
            {
                string filename = Console.ReadLine();

                if (String.IsNullOrEmpty(filename))
                    break;

                string folder = GetFolderName(filename);

                CreateFolder(folder);

                Download(blobFolder, filename, filename);
            }
        }

        static string GetFolderName(string filename)
        {
            return Path.GetDirectoryName(filename);
        }

        static void CreateFolder(string foldername)
        {
            if (foldername == "..")
                throw new InvalidOperationException();

            if (foldername == "" || foldername == ".")
                return;

            Directory.CreateDirectory(foldername);
        }

        static void Download(BlobFolder folder, string inputname, string outputname)
        {
            using (var input = folder.OpenRead(inputname))
            {
                using (var output = File.OpenWrite(outputname))
                {
                    input.CopyTo(output);
                }
            }
        }
    }
}
