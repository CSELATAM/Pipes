using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Pipes.Azure
{
    public class StorageQueue
    {
        class Message : IDisposable
        {
            public readonly CloudQueueMessage InternalMessage;
            private CloudQueue _queue;
            private bool _isDisposed = false;

            public Message(CloudQueue queue, CloudQueueMessage message)
            {
                this._queue = queue;
                this.InternalMessage = message;
            }

            public string Content => InternalMessage.AsString;

            void UpdateMessageVisibility(int seconds)
            {
                _queue.UpdateMessageAsync(InternalMessage, TimeSpan.FromSeconds(seconds), MessageUpdateFields.Visibility);
            }

            public void Cancel()
            {
                if (_isDisposed)
                    throw new InvalidOperationException("Cancel(): Message already in Disposed state");

                try { UpdateMessageVisibility(0); }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                _queue = null;
                _isDisposed = true;
            }

            public void Done()
            {
                if (_isDisposed)
                    throw new InvalidOperationException("Done(): Message already in Disposed state");

                _queue.DeleteMessageAsync(InternalMessage).Wait();
                _queue = null;
                _isDisposed = true;
            }

            public void Dispose()
            {
                if (_isDisposed)
                    return;

                try
                {
                    Done();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
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

            return (message != null) ? new Message(_queue, message) : null;
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
