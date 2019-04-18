using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Http.Description;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using CourseworkPartB.Models;


namespace CourseworkPartBSoap
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        private const String partitionName = "File_Partition_1";

        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable table;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        public Service1()
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference("Files");
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference("pdfgallery");
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
        public List<GetALL> GetAllRecords()
        {
            TableQuery<FileMetadataEnitity> query = new TableQuery<FileMetadataEnitity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionName));
            List<FileMetadataEnitity> entityList = new List<FileMetadataEnitity>(table.ExecuteQuery(query));

            // Basically create a list of Product from the list of ProductEntity with a 1:1 object relationship, filtering data as needed
            var filesList = from e in entityList
                            select new GetALL()
                            {
                                FileMetadataID = e.RowKey,
                                Title = e.Title,
                                CreationDate = e.CreationDate,
                            };
            return filesList.ToList();
        }
        [ResponseType(typeof(FileMetadataSummaryDTO))]
        public Stream GetCertain(string id)
        {
            TableOperation getOperation = TableOperation.Retrieve<FileMetadataEnitity>(partitionName, id);
            TableResult getOperationResult = table.Execute(getOperation);
            FileMetadataEnitity fileEntity = (FileMetadataEnitity)getOperationResult.Result;
            FileMetadataSummaryDTO p = new FileMetadataSummaryDTO()
            {
                FileMetadataID = fileEntity.RowKey,
                Title = fileEntity.Title,
                CreationDate = fileEntity.CreationDate,
                PDFFileBlobURL = fileEntity.PDFFileBlobURL
            };
            string url1 = p.PDFFileBlobURL.ToString().Substring(p.PDFFileBlobURL.LastIndexOf('/') + 1);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference("pdf/" + url1);
            Stream blobStream = blob.OpenRead();
            return blobStream;
        }

    }
}

