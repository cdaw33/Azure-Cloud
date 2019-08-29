using System;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GlobalClassesLibrary
{
    public class BlobMetadata
    {
        public static String getBlobMetaData(Uri blobUri)
        {
            //Adds Metadata to the Blob data so that the Title can be seen
            CloudBlockBlob blob = new CloudBlockBlob(blobUri);
            blob.FetchAttributes();
            return blob.Metadata["Title"];
        }
    }
}
