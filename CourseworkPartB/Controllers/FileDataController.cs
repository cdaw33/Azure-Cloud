using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GlobalClassesLibrary;

namespace CourseworkPartB.Controllers
{
    public class FileDataController : ApiController
    {
        [ResponseType(typeof(FileMetadataSummaryDTO))]
        public HttpResponseMessage Get(string id)
        {
            //Creates a new error message for if the id is not in the table
            HttpError notFound = new HttpError("Error File Not Found");
            //Creates new Http Response message with the status OK to be passed to the global class library
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            //Checks to see if the id is in the table and returns a error if it is not
            if (GetTable.GetTableOperation(id) == "error") return Request.CreateErrorResponse(HttpStatusCode.BadRequest, notFound);
            else
            {
                //calls the getcertainrest function from the class library and returns the message created
                return GetCertainID.getCertainRest(id, message);

            }
        }
        }
    }


