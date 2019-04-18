using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.IO;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;

namespace CourseworkPartBSoap
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<GetALL> GetAllRecords();

        [OperationContract]
        [WebGet(BodyStyle =WebMessageBodyStyle.Bare, UriTemplate ="{id}")]
        Stream GetCertain(string id);
        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "CourseworkPartB_SOAP.ContractType".
    [DataContract]
    public class GetALL
    {
        [DataMember]
        public string FileMetadataID { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string CreationDate { get; set; }

    }
    
}
