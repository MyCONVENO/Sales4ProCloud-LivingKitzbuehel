//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudDataService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Association
    {
        public string AssociationID { get; set; }
        public string AssociationName1 { get; set; }
        public string AssociationName2 { get; set; }
        public string AssociationName3 { get; set; }
        public string AssociationStreet { get; set; }
        public string AssociationZIP { get; set; }
        public string AssociationCity { get; set; }
        public string AssociationEMail { get; set; }
        public string AssociationPhone { get; set; }
        public string AssociationFax { get; set; }
        public int AssociationPricelistID { get; set; }
        public System.DateTime SyncDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string AssociationCountryName { get; set; }
        public string AssociationCountryCode { get; set; }
        public string AssociationClientID { get; set; }
        public Nullable<bool> IsTestData { get; set; }
    }
}
