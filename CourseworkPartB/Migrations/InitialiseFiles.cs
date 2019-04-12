using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Configuration;
using CourseworkPartB.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CourseworkPartB.Migrations
{
    public class InitialiseFiles
    {
        public static void go()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("Files");


        }
    }
}