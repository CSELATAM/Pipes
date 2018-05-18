using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pipes.Azure
{
    public class StorageQueue
    {
        class Message
        {
            public readonly CloudQueueMessage InternalMessage;

            public Message(CloudQueueMessage message)
            {
                this.InternalMessage = message;
            }

            public string Content => InternalMessage.AsString;
        }

        private readonly CloudQueue _queue;

        public StorageQueue(string queueUrl)
        {
            this._queue = new CloudQueue(new Uri(queueUrl));
        }

        Message TryReadQueue()
        {
            var message = _queue.GetMessageAsync().Result;

            return (message != null) ? new Message(message) : null;
        }

        void DeleteMessage(Message message)
        {
            _queue.DeleteMessageAsync(message.InternalMessage);
        }

        public string Read()
        {
            const int BACKOFF_INITIAL = 500;
            const int BACKOFF_THRESHOLD = 60000;
            const int BACKOFF_MULTIPLIER = 2;

            var message = TryReadQueue();
            int backoff = BACKOFF_INITIAL;

            while(message == null)
            {
                Task.Delay(backoff).Wait();

                if (backoff < BACKOFF_THRESHOLD)
                {
                    backoff *= BACKOFF_MULTIPLIER;
                }

                message = TryReadQueue();
            }

            string content = message.Content;

            DeleteMessage(message);

            return content;
        }
    }
}
