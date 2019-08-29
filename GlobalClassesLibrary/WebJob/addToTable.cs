using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GlobalClassesLibrary.WebJob
{
    public class addToTable
    {
        public static void AddtoTable(string title, string created, string TextFileURL, string PDFFileURL)
        {
            //checks to see if the Files table exists and creates it if it does not
            Variables.table.CreateIfNotExists();
            //creates a string that will hold the number of files in the table
            String getNewMaxRowKeyValue;
            //creates a query to the table
            TableQuery<FileMetadataEntity> query = new TableQuery<FileMetadataEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Variables.partitionName));
            //finds how many files are in the table
            int maxRowKeyValue = 0;
            foreach (FileMetadataEntity entity in Variables.table.ExecuteQuery(query))
            {
                int entityRowKeyValue = Int32.Parse(entity.RowKey);
                if (entityRowKeyValue > maxRowKeyValue) maxRowKeyValue = entityRowKeyValue;
            }
            maxRowKeyValue++;
            getNewMaxRowKeyValue = maxRowKeyValue.ToString();
            //creates a new entry to be added into the table
            FileMetadataEntity newFile = new FileMetadataEntity(Variables.partitionName, getNewMaxRowKeyValue);
            newFile.Title = title;
            newFile.CreationDate = created;
            newFile.PDFFileBlobURL = PDFFileURL;
            newFile.TextFileBlobURL = TextFileURL + ".txt";
            newFile.FileMetadataID = newFile.RowKey;
            //Adds the created file into the table
            TableBatchOperation batchOperation = new TableBatchOperation();
            batchOperation.Insert(newFile);
            Variables.table.ExecuteBatch(batchOperation);

        }
    }
}
