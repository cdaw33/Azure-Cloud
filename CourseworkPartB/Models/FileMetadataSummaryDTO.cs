// This is a Data Transfer Object (DTO) class. This is sent/received in REST requests/responses.
// Read about DTOS here: https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5

using System.ComponentModel.DataAnnotations;
using System;

namespace CourseworkPartB.Models
{
    public class FileMetadataSummaryDTO
    {
        /// <summary>
        /// FileMetadataID
        /// </summary>
        [Key]
        public string FileMetadataID { get; set; }

        /// <summary>
        /// Name of file
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Date Created
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// Date Created
        /// </summary>
        public string PDFFileBlobURL { get; set; }



    }
}