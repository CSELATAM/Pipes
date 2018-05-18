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
        private TimeSpan DEFAULT_MESSAGE_VISIBILITY = TimeSpan.FromSeconds(10000);

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
                _queue.UpdateMessageAsync(InternalMessage, TimeSpan.FromSeconds(seconds), MessageUpdateFields.Visibility).Wait();
            }

            public void Cancel()
            {
                if (_isDisposed)
                    throw new InvalidOperationException("Cancel(): Message already in Disposed state");

                TryCatch(() => UpdateMessageVisibility(0));

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

                TryCatch( () => Done() );
            }
        }

        private readonly CloudQueue _queue;

        public StorageQueue(string queueUrl)
        {
            this._queue = new CloudQueue(new Uri(queueUrl));
        }

        Message TryReadQueue()
        {
            var message = _queue.GetMessageAsync(DEFAULT_MESSAGE_VISIBILITY, options: null, operationContext: null).Result;

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
            string message = TryReadQueueString();
            int backoff = 0;

            while (message == null)
            {
                WaitExponential(ref backoff);
                
                // retry
                message = TryReadQueueString();
            }
            
            return message;
        }

        public void Send(string message)
        {
            var cloudMessage = new CloudQueueMessage(message);

            _queue.AddMessageAsync(cloudMessage).Wait();
        }

        static void TryCatch(Action cmd)
        {
            try
            {
                cmd();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void WaitExponential(ref int backoff)
        {
            const int BACKOFF_INITIAL = 500;
            const int BACKOFF_THRESHOLD = 60000;
            const int BACKOFF_MULTIPLIER = 2;

            if( backoff == 0 )
                backoff = BACKOFF_INITIAL;

            Task.Delay(backoff).Wait();

            if (backoff < BACKOFF_THRESHOLD)
            {
                backoff *= BACKOFF_MULTIPLIER;
            }
        }
    }
}
