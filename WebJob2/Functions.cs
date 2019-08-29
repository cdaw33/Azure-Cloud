using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using GlobalClassesLibrary.WebJob;

namespace WebJob2
{
    public class Functions
    {
        public static void GeneratePDF(
        //QueueTrigger fires when there is a message detected in the queue
        //Stores the data from the queue in a string
        //Places the user submitted document into the container in the "documents" directory
        //Places the converted file into the container and stores it as .pdf
        [QueueTrigger("pdfmaker")] String blobInfo,
        [Blob("pdfgallery/documents/{queueTrigger}")] CloudBlockBlob inputBlob,
        [Blob("pdfgallery/pdf/{queueTrigger}.pdf")] CloudBlockBlob outputBlob,
        TextWriter logger)
        {
            
            //Generates output to show that the blob has been processed
            logger.WriteLine("GeneratePDF() started...");
            logger.WriteLine("Input blob is: " + blobInfo);
            inputBlob.FetchAttributes();
            //sets the metadata title
            string blobName = outputBlob.Name.ToString();
            string blobTitle = outputBlob.Name.ToString().Substring(blobName.LastIndexOf('/') + 1);
            //Retrives the name of the file to be stored in the Azure Table
            string fileName = inputBlob.Name.ToString();
            string fileTitle = inputBlob.Name.ToString().Substring(fileName.LastIndexOf('/') + 1);
            //Stores the date that the file was uploaded to be stored in the table
            string dateCreated = inputBlob.Properties.LastModified.ToString();
            //Stores both the txt file URL and the newly created PDF file URL
            string textURL = inputBlob.StorageUri.PrimaryUri.ToString();
            string pdfURL = outputBlob.StorageUri.PrimaryUri.ToString();
            //Adds the above data into an Azure table by calling the AddtoTable function from the class library
            addToTable.AddtoTable(fileTitle, dateCreated, textURL, pdfURL);
            
            // Open streams to blobs for reading and writing
            using (Stream input = inputBlob.OpenRead())
            using (Stream output = outputBlob.OpenWrite())
            {
                //Calls the conversion function from the class library and passes the user file using the input stream
                ConvertToPDF.ConvertDocToPDF(input, output);
                outputBlob.Properties.ContentType = "document/pdf";
                //Adds metadata to the blob
                outputBlob.Metadata["Title"] = blobTitle;
                logger.WriteLine("GeneratePDF() completed...");
            }

        }
        

    }
}