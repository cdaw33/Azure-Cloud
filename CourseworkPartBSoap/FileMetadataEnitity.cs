using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace CourseworkPartBSoap
{
    public class FileMetadataEnitity : TableEntity
    {
        public string Title { get; set; }
        public string CreationDate { get; set; }
        public string TextFileBlobURL { get; set; }
        public string PDFFileBlobURL { get; set; }
        public string FileMetadataID { get; set; }
        public FileMetadataEnitity(string partitionKey, string fileID)
        {
            PartitionKey = partitionKey;
            RowKey = fileID;
        }

        public FileMetadataEnitity() { }

    }
}
