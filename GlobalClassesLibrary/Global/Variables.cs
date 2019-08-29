using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GlobalClassesLibrary
{
    public class Variables
    {
        //Creates a string to hold the partition to seperate each record
        public const String partitionName = "File_Partition_1";
        //Creates a connection to Azure Storage
        public static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
        //Creates a new connection to the table client
        public static CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
        //Access the right table
        public static CloudTable table = tableClient.GetTableReference("Files");
        //Creates a new connection to the blob client
        public static CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //Access the right blob
        public static CloudBlobContainer blobContainer = blobClient.GetContainerReference("pdfgallery");

        public class Filesummary
        {
            public string FileMetadataID { get; set; }
            public string Title { get; set; }
            public string CreationDate { get; set; }
        }
    }
}
