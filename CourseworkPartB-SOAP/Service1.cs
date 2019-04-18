using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.Services.Client;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;

namespace CourseworkPartB_SOAP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable table;
        public Service1()
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference("Files");
        }
        
    }
}
    


