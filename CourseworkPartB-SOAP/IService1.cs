using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CourseworkPartB_SOAP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        List<GetALL> GetAllRecords();

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
