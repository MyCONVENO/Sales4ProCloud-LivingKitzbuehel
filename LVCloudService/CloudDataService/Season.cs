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
    
    public partial class Season
    {
        public Season()
        {
            this.Model = new HashSet<Model>();
        }
    
        public string SeasonID { get; set; }
        public string SeasonName { get; set; }
        public string SeasonLabelID { get; set; }
        public int SeasonSort { get; set; }
        public string SeasonNumber { get; set; }
        public System.DateTime SyncDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string SeasonClientID { get; set; }
        public Nullable<bool> IsTestData { get; set; }
        public string SeasonLongName { get; set; }
        public Nullable<System.DateTime> DeliveryDateStart { get; set; }
        public Nullable<System.DateTime> DeliveryDateEnd { get; set; }
    
        public virtual Label Label { get; set; }
        public virtual ICollection<Model> Model { get; set; }
    }
}
