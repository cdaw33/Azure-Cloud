using System.Net.Http;
using System.IO;
using System.ServiceModel.Web;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GlobalClassesLibrary
{
    public class GetCertainID
    {
        public static Stream getCertainSoap(string url)
        {                
            //Creates a Cloud Block Blob that holds the details of the correct file
            CloudBlockBlob blob = Variables.blobContainer.GetBlockBlobReference("pdf/" + url);
            Stream blobStream = blob.OpenRead();
            //Adds headers to the soap message
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/pdf";
            WebOperationContext.Current.OutgoingResponse.Headers.Add("content-disposition", "attachment; filename=" + url);
            WebOperationContext.Current.OutgoingResponse.ContentLength = blobStream.Length;
            return blobStream;
        }


        public static HttpResponseMessage getCertainRest(string id, HttpResponseMessage message)
        {
            string url1 = GetTable.GetTableOperation(id).Substring(GetTable.GetTableOperation(id).LastIndexOf('/') + 1);
            CloudBlockBlob blob = Variables.blobContainer.GetBlockBlobReference("pdf/" + url1);
            //Opens up a new stream to read the file
            Stream blobStream = blob.OpenRead();
            //Creates a http message and fills it with the blob file
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
            //returns the HttpResponseMessage
            return message;
        }
    }
}
