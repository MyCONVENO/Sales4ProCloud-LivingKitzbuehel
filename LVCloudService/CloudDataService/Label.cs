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
    
    public partial class Label
    {
        public Label()
        {
            this.Season = new HashSet<Season>();
        }
    
        public string LabelID { get; set; }
        public string LabelName { get; set; }
        public string LabelNumber { get; set; }
        public System.DateTime SyncDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string LabelClientID { get; set; }
        public Nullable<bool> IsTestData { get; set; }
    
        public virtual ICollection<Season> Season { get; set; }
    }
}
