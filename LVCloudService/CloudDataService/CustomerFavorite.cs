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
    
    public partial class CustomerFavorite
    {
        public string CustomerFavoriteID { get; set; }
        public string UserID { get; set; }
        public string CustomerID { get; set; }
        public bool IsFavorite { get; set; }
        public System.DateTime SyncDateTime { get; set; }
        public Nullable<bool> IsTestData { get; set; }
        public long SyncDateTimeSort { get; set; }
    }
}
