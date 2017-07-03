using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CloudDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ILVKBImportService
    {

        [OperationContract]
        string ImportSourceData();

        [OperationContract]
        string ImportArticle();

        [OperationContract]
        string ImportCustomer();

        [OperationContract]
        string ImportPrice();

        [OperationContract]
        string UpdateGeoData();

        [OperationContract]
        string ImportSourceDataStock();

        [OperationContract]
        string ImportStock();

        [OperationContract]
        string ImportAllData();

        [OperationContract]
        string ImportAllDataStock();

        [OperationContract]
        string ImportSeasonValue();
    }
}
