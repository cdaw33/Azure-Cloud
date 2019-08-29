using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GlobalClassesLibrary;


namespace CourseworkPartB.Controllers
{
    public class FileMetadataController : ApiController
    {
        /// <summary>
        /// Get all files
        /// </summary>
        /// <returns></returns>
        // GET: api/FileMetadata
        public IEnumerable<Filesummary> Get()
        {
            //creates a list using the GetAllTable function from the class library
            var filesList = from e in GetTable.GetAllTable()
                            select new Filesummary()
                            {
                                FileMetadataID = e.FileMetadataID,
                                Title = e.Title,
                                CreationDate = e.CreationDate,
                            };
            return filesList;
        }
    }
}