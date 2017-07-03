using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudDataService.ImportClass
{
    public class ImportAssociationMember
    {
        public string AssociationMemberID { get; set; }
        public string AssociationID { get; set; }
        public string CustomerID { get; set; }
        public string EKV_KTO { get; set; }
    }
}