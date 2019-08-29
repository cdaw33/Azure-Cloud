using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;

namespace GlobalClassesLibrary
{
    public class GetTable
    {
        public static List<Filesummary> GetAllTable()
        {
            //Querys the table using the Entitiy class as a basis
            TableQuery<FileMetadataEntity> query = new TableQuery<FileMetadataEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Variables.partitionName));
            List<FileMetadataEntity> entityList = new List<FileMetadataEntity>(Variables.table.ExecuteQuery(query));
            //Creates a list of all the files stored in the table and displays certain attributes
            var filesList = from e in entityList
                            select new Filesummary()
                            {
                                FileMetadataID = e.RowKey,
                                Title = e.Title,
                                CreationDate = e.CreationDate,
                            };
            //Returns a list
            return filesList.ToList();
        }
        public static string GetTableOperation(string id)
            {
            //retrieves the file from the table using the id
            TableOperation getOperation = TableOperation.Retrieve<FileMetadataEntity>(Variables.partitionName, id);
            TableResult getOperationResult = Variables.table.Execute(getOperation);
            //Checks to see if the id is in the table
            if (getOperationResult.Result == null)
            {
                string error = "error";
                return error;
            }
            else
            {
                //uses FileMetadataEntity from the class library
                FileMetadataEntity fileEntity = (FileMetadataEntity)getOperationResult.Result;
                //stores the file data in a Data Transfer Object
                FileMetadataSummaryDTO p = new FileMetadataSummaryDTO()
                {
                    FileMetadataID = fileEntity.RowKey,
                    Title = fileEntity.Title,
                    CreationDate = fileEntity.CreationDate,
                    PDFFileBlobURL = fileEntity.PDFFileBlobURL
                };
                //returns the PDF Url
                return p.PDFFileBlobURL;
            }
            }

    }
}
