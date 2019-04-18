using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ClassLibrary1
{
    public class FileMetadataEntity : TableEntity
    {
        public string Title { get; set; }
        public string CreationDate { get; set; }
        public string TextFileBlobURL { get; set; }
        public string PDFFileBlobURL { get; set; }
        public string FileMetadataID { get; set; }
        public FileMetadataEntity(string partitionKey, string fileID)
        {
            PartitionKey = partitionKey;
            RowKey = fileID;
        }

        public FileMetadataEntity() { }

    }
}
