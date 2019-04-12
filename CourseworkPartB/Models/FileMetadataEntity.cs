// Entity class for Azure table
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ProductStore.Models
{

    public class FileMetadataEntity : TableEntity
    {
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public string TextFileBlobURL { get; set; }
        public string PDFFileBlobURL { get; set; }


        public FileMetadataEntity(string partitionKey, string fileID)
        {
            PartitionKey = partitionKey;
            RowKey = fileID;
        }

        public FileMetadataEntity() { }

    }
}
