using System;
using System.Collections.Generic;
using Pipes.Azure;
using Pipes.Core;

namespace ReadQueue
{
    // ReadQueue: Arguments
    class ReadQueueArgs : PipeArgs
    {
        //
        // Syntax: readqueue <storageUrl>
        //
        public string StorageUrl => Argument(0);
    }

    // ReadQueue: Implementation
    class ReadQueueMain : PipeMain<ReadQueueArgs>
    {
        StorageQueue _queue;

        public override void Initialize(ReadQueueArgs args)
        {
            _queue = new StorageQueue(args.StorageUrl);
        }

        //Read the messages from the queue
        public override PipeOutput RunBefore()
        {
            return PipeOutput.From(MessagesFromQueue());
        }

        IEnumerable<string> MessagesFromQueue()
        {
            while (true)
            {
                var message = _queue.Read();

                if (String.IsNullOrEmpty(message))
                    break;

                yield return message;
            }
        }
    }

    class Program
    {
        static int Main(string[] args) => (new ReadQueueMain()).RunWith(args);
    }
}
