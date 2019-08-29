using System;
using System.IO;
using System.Linq;
using GlobalClassesLibrary;

namespace WebAppInterface
{
    public partial class _Default : System.Web.UI.Page
    {
        private string GetMimeType(string Filename)
        {
            //Gets the MimeType of the file
            try
            {
                string ext = Path.GetExtension(Filename).ToLowerInvariant();
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                if (key != null)
                {
                    string contentType = key.GetValue("Content Type") as String;
                    if (!String.IsNullOrEmpty(contentType))
                    {
                        return contentType;
                    }
                }
            }
            catch
            {
            }
            return "application/octet-stream";
        }

        protected void submitButton_Click(object sender, EventArgs e)
        {
            //Checks if the user has uploaded a file
            if (upload.HasFile)
            {
                //Gets the name of the extension of the file
                var extension = Path.GetExtension(upload.FileName);
                //Checks to make sure that it is a .txt file that has been uploaded
                if (extension == ".txt")
                {
                    //Makes sure the error message is not visible
                    ErrorMsg.Visible = false;
                    //Stores the filename submitted by the user without the file extension 
                    //in order to make it easier to convert
                    var ext = Path.GetFileNameWithoutExtension(upload.FileName);

                    //Sets the path to the blob container to store the user submitted file
                    String path = "documents/" + ext;

                    //Contacts the BlobStorageService passing along the path of the user file in the Blob
                    var blob = BlobStorageService.getCloudBlobContainer().GetBlockBlobReference(path);

                    //Set the MIME type
                    blob.Properties.ContentType = GetMimeType(upload.FileName);

                    //Uploads the data in the user submitted file to the Blob
                    blob.UploadFromStream(upload.FileContent);

                    //Sends a message to the queue to activate the PDF converter
                    CloudQueueService.QueueMessage(ext);

                    System.Diagnostics.Trace.WriteLine(String.Format("*** WebRole: Enqueued '{0}'", path));
                }
                else
                {
                    //Displays error message if it isnt a .txt that has been uploaded
                    ErrorMsg.Visible = true;
                }
            }
        }

        protected void refresh_Click(object sender, EventArgs e)
        {
            try
            {
                //Fills The asp:ListView with the data that is contained in the pdf directory blob
                //Takes both the URL of the file and the metadata title
                PDFDisplayControl.DataSource = from o in BlobStorageService.getCloudBlobContainer().GetDirectoryReference("pdf").ListBlobs()
                                               select new
                                               {
                                                   Url = o.Uri,
                                                   Title = BlobMetadata.getBlobMetaData(o.Uri)
                                               };


                //Binds the data to the ListView so that it displays
                PDFDisplayControl.DataBind();
            }
            catch (Exception)
            {
            }
        }

    }
}