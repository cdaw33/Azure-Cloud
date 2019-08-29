using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace GlobalClassesLibrary
{
    public class BlobStorageService
    {
        public static CloudBlobContainer getCloudBlobContainer()
        {
            //Creates a connection the Azure Storage
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse
                (ConfigurationManager.ConnectionStrings["AzureStorage"].ToString());

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer blobContainer = blobClient.GetContainerReference("pdfgallery");
            if (blobContainer.CreateIfNotExists())
            {
                // Enable public access on the newly created container.
                blobContainer.SetPermissions(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
            }
            return blobContainer;
        }
    }
}