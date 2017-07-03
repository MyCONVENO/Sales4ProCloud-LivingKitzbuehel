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
    
    public partial class Pricelist
    {
        public Pricelist()
        {
            this.Price = new HashSet<Price>();
            this.UserPriceList = new HashSet<UserPriceList>();
            this.Customer = new HashSet<Customer>();
        }
    
        public string PricelistID { get; set; }
        public string PricelistName { get; set; }
        public string PricelistNumber { get; set; }
        public string Currency { get; set; }
        public bool IsDefault { get; set; }
        public System.DateTime SyncDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string PricelistClientID { get; set; }
        public Nullable<bool> IsTestData { get; set; }
    
        public virtual ICollection<Price> Price { get; set; }
        public virtual ICollection<UserPriceList> UserPriceList { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
    }
}
