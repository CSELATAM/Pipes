using Pipes.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pipes.Core
{
    class ProgramExample
    {
        public static void Main(string[] args) => (new Example()).RunWith(args);
    }

    public class ExampleArgs : PipeArgs
    {
        // Syntax:
        // example <storage_url> [storage_type] [-v]
        //
        string StorageUrl => Argument(0);
        string StorageType => Optional(1);

        // Additional parameters
        //   -v              logging
        //   -c <filename>   create..
        //
        string LogVerbose => Flag("--verbose", "-v");
    }

    class Example : PipeMain<ExampleArgs>
    {
        public override void Initialize(ExampleArgs args)
        {
        }
        
        public override PipeOutput Run(PipeInput input)
        {
            string filename = input;

            string folder = GetFolderName(filename);

            CreateFolder(folder);

            return PipeOutput.FromString(filename);
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
    }
}
