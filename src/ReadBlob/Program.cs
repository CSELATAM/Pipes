using Pipes.Azure;
using Pipes.Core;
using System;
using System.IO;

namespace ReadBlob
{
    class ReadBlobArgs : PipeArgs
    {
        public string StorageUrl => Argument(0);
    }

    class ReadBlobMain : PipeMain<ReadBlobArgs>
    {
        BlobFolder _folder;

        public override void Initialize(ReadBlobArgs args)
        {
            _folder = new BlobFolder(args.StorageUrl);            
        }

        public override PipeOutput Run(PipeInput input)
        {
            string blobName = input.GetString();

            using (var stream = _folder.OpenRead(blobName))
            using(var reader = new StreamReader(stream))
            {
                string content = reader.ReadToEnd();

                return PipeOutput.RemoveCRLF(content);
            }
        }
    }

    class Program
    {
        static int Main(string[] args) => (new ReadBlobMain()).RunWith(args);
    }
}
