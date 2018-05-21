using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pipes.Azure
{
    public class BlobFolder
    {
        private readonly CloudBlobContainer _container;

        public BlobFolder(string connectionString, string container)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudBlobClient();

            this._container = client.GetContainerReference(container);
        }

        public BlobFolder(string url)
        {
            var container = new CloudBlobContainer(new Uri(url));

            this._container = container;
        }

        BlobResultSegment ListBlobsSegmented(CloudBlobContainer container, string prefix, bool flatListing, BlobContinuationToken token)
        {
            return container.ListBlobsSegmentedAsync(
                    prefix: prefix,
                    useFlatBlobListing: flatListing,
                    blobListingDetails: BlobListingDetails.None,
                    maxResults: 1000,
                    currentToken: token,
                    options: null,
                    operationContext: null).Result;
        }

        public IEnumerable<string> EnumItems(string prefix, bool recursive = true)
        {
            BlobContinuationToken token = null;
            
            do
            {
                var segment = ListBlobsSegmented(_container, prefix, recursive, token);

                foreach (var blob in segment.Results)
                {
                    var file = blob as CloudBlockBlob;

                    if (file == null)
                        continue;

                    yield return file.Name;
                }

                token = segment.ContinuationToken;

            } while (token != null);
        }

        public Stream OpenRead(string filename)
        {
            var blob = _container.GetBlobReference(filename);

            return blob.OpenReadAsync().Result;
        }
    }
}
