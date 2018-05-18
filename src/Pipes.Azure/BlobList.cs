using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pipes.Azure
{
    class BlobList
    {
        BlobResultSegment ListBlobsSegmented(CloudBlobContainer container, string prefix, bool recursive, BlobContinuationToken token)
        {
            return container.ListBlobsSegmentedAsync(
                    prefix: prefix,
                    useFlatBlobListing: false,
                    blobListingDetails: BlobListingDetails.None,
                    maxResults: 1000,
                    currentToken: token,
                    options: null,
                    operationContext: null).Result;
        }

        public IEnumerable<string> EnumItems(CloudBlobContainer container, string prefix, bool recursive)
        {
            BlobContinuationToken token = null;

            do
            {
                var segment = ListBlobsSegmented(container, prefix, recursive, token);

                foreach (var blob in segment.Results)
                {
                    yield return blob.Uri.AbsolutePath;
                }

                token = segment.ContinuationToken;

            } while (token != null);
        }
    }
}
