using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using GlobalClassesLibrary;

namespace CourseworkPartA_WebJob
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
            string fileName = inputBlob.Name.ToString();
            string fileTitle = inputBlob.Name.ToString().Substring(fileName.LastIndexOf('/') + 1);
            string dateCreated = inputBlob.Properties.LastModified.ToString();
            string textURL = inputBlob.StorageUri.PrimaryUri.ToString();
            string pdfURL = outputBlob.StorageUri.PrimaryUri.ToString();
            foreach (var metadataKey in inputBlob.Metadata.Keys)
            {
                outputBlob.Metadata["Title"] = metadataKey.ToString();
                outputBlob.SetMetadata();
            }
            AddtoTable(fileTitle, dateCreated, textURL, pdfURL);
            // Open streams to blobs for reading and writing
            using (Stream input = inputBlob.OpenRead())
            using (Stream output = outputBlob.OpenWrite())
            {
                //Calls the conversion function and passes the user file using the input stream
                ConvertDocToPDF(input, output);
                outputBlob.Properties.ContentType = "document/pdf";
                //Adds metadata to the blob
                outputBlob.Metadata["Title"] = blobTitle;
                logger.WriteLine("GeneratePDF() completed...");
            }

        }
        public static void AddtoTable(String title, string created, string TextFileURL, string PDFFileURL)
        {
            const String partitionName = "File_Partition_1";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("Files");

            table.CreateIfNotExists();


            String getNewMaxRowKeyValue;

            TableQuery<FileMetadataEntity> query = new TableQuery<FileMetadataEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionName));

            int maxRowKeyValue = 0;
            foreach (FileMetadataEntity entity in table.ExecuteQuery(query))
            {
                int entityRowKeyValue = Int32.Parse(entity.RowKey);
                if (entityRowKeyValue > maxRowKeyValue) maxRowKeyValue = entityRowKeyValue;
            }
            maxRowKeyValue++;
            getNewMaxRowKeyValue = maxRowKeyValue.ToString();
            FileMetadataEntity newFile = new FileMetadataEntity(partitionName, getNewMaxRowKeyValue);
            newFile.Title = title;
            newFile.CreationDate = created;
            newFile.PDFFileBlobURL = PDFFileURL;
            newFile.TextFileBlobURL = TextFileURL + ".txt";
            newFile.FileMetadataID = newFile.RowKey;

            TableBatchOperation batchOperation = new TableBatchOperation();
            batchOperation.Insert(newFile);
            table.ExecuteBatch(batchOperation);

        }

        public static void ConvertDocToPDF(Stream input, Stream output)
        {

            //This function worked converting locally however did not work on the cloud
            // Document doc = new Document();
            // doc.LoadFromStream(input, Spire.Doc.FileFormat.Txt);
            // doc.SaveToStream(output, Spire.Doc.FileFormat.PDF);

            //This code has been apdated from the Spire.PDF website
            //Creates a streamreader in order to read the data held in the input stream
            StreamReader reader = new StreamReader(input);
            //Reads all the file
            string doc2 = reader.ReadToEnd();
            //Creates a new instance of a Spire.PDF document and adds section pages and formatting to it
            PdfDocument doc = new PdfDocument();
            PdfSection section = doc.Sections.Add();
            PdfPageBase page = section.Pages.Add();
            PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 11);
            PdfStringFormat format = new PdfStringFormat();
            format.LineSpacing = 20f;
            PdfBrush brush = PdfBrushes.Black;
            PdfTextWidget textWidget = new PdfTextWidget(doc2, font, brush);
            float y = 0;
            PdfTextLayout textLayout = new PdfTextLayout();
            textLayout.Break = PdfLayoutBreakType.FitPage;
            textLayout.Layout = PdfLayoutType.Paginate;
            RectangleF bounds = new RectangleF(new PointF(0, y), page.Canvas.ClientSize);
            textWidget.StringFormat = format;
            textWidget.Draw(page, bounds, textLayout);
            //Saves the file as a stream using the output stream variable and converts it to a PDF
            doc.SaveToStream(output, Spire.Pdf.FileFormat.PDF);

        }
    }
}