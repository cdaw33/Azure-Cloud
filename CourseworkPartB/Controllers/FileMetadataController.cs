using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using CourseworkPartB.Models;


namespace CourseworkPartB.Controllers
{
    public class FileMetadataController : ApiController
    {
        private const String partitionName = "File_Partition_1";

        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable table;
        public FileMetadataController()
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference("Files");
        }
        public class Filesummary
        {
            public string FileMetadataID { get; set; }
            public string Title { get; set; }
            public string CreationDate { get; set; }
        }
        /// <summary>
        /// Get all files
        /// </summary>
        /// <returns></returns>
        // GET: api/FileMetadata
        public IEnumerable<Filesummary> Get()
        {
            TableQuery<FileMetadataEntity> query = new TableQuery<FileMetadataEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionName));
            List<FileMetadataEntity> entityList = new List<FileMetadataEntity>(table.ExecuteQuery(query));

            // Basically create a list of Product from the list of ProductEntity with a 1:1 object relationship, filtering data as needed
            var filesList = from e in entityList
                            select new Filesummary()
                            {
                                FileMetadataID = e.RowKey,
                                Title = e.Title,
                                CreationDate = e.CreationDate,
                            };
            return filesList;
        }
    }
}
