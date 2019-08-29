using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Configuration;
using System.Diagnostics;

namespace GlobalClassesLibrary
{
    public class CloudQueueService
    {
        public static CloudQueue getCloudQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse
                 (ConfigurationManager.ConnectionStrings["AzureStorage"].ToString());

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            CloudQueue sampleQueue = queueClient.GetQueueReference("pdfmaker");
            sampleQueue.CreateIfNotExists();

            Trace.TraceInformation("Queue initialized");

            return sampleQueue;
        }

        public static void QueueMessage(string name)
        {
            //Adds a message to the queue
            getCloudQueue().AddMessage(new CloudQueueMessage(System.Text.Encoding.UTF8.GetBytes(name)));
        }
    }
}