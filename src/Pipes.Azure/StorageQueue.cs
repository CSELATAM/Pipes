using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pipes.Azure
{
    public class StorageQueue
    {
        class Message : IDisposable
        {
            public readonly CloudQueueMessage InternalMessage;
            private readonly CloudQueue _queue;
            private bool _isDisposed = false;

            public Message(CloudQueueMessage message)
            {
                this.InternalMessage = message;
            }

            public string Content => InternalMessage.AsString;

            public void Done()
            {
                if(!_isDisposed)
                {
                    _isDisposed = true;
                    _queue.DeleteMessageAsync(InternalMessage).Wait();
                }
            }

            public void Dispose()
            {
                try
                {
                    Done();
                }
                catch { }
            }
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

        string TryReadQueueString()
        {
            using (var message = TryReadQueue())
            {
                if (message == null)
                    return null;

                return message.Content;
            }
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

            int backoff = BACKOFF_INITIAL;

            string message = TryReadQueueString();

            while (message == null)
            {
                Task.Delay(backoff).Wait();

                if (backoff < BACKOFF_THRESHOLD)
                {
                    backoff *= BACKOFF_MULTIPLIER;
                }

                message = TryReadQueueString();
            }
            
            return message;
        }
    }
}
