using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using CourseworkPartB.Models;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;


namespace CourseworkPartB.Controllers
{
    public class FileDataController : ApiController
    {
        private const String partitionName = "File_Partition_1";

        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable table;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        public FileDataController()
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference("Files");
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference("pdfgallery");
        }

        [ResponseType(typeof(FileMetadataSummaryDTO))]
        public HttpResponseMessage Get(string id)
        {
            TableOperation getOperation = TableOperation.Retrieve<FileMetadataEntity>(partitionName, id);
            TableResult getOperationResult = table.Execute(getOperation);
            HttpError notFound = new HttpError("Error File Not Found");
            if (getOperationResult.Result == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, notFound);
            else
            {
                FileMetadataEntity fileEntity = (FileMetadataEntity)getOperationResult.Result;
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
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
                message.Content = new StreamContent(blobStream);
                message.Content.Headers.ContentLength = blob.Properties.Length;
                message.Content.Headers.ContentType = new
                System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                message.Content.Headers.ContentDisposition = new
                System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = blob.Name,
                    Size = blob.Properties.Length
                };
                return message; // exception handling omitted...
            }
        }
    }
}
