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
    
    public partial class UserAgents
    {
        public string UserAgentsID { get; set; }
        public string UserID { get; set; }
        public string AgentID { get; set; }
        public string UserAgentClientID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> SyncDateTime { get; set; }
        public long SyncDateTimeSort { get; set; }
    
        public virtual Agent Agent { get; set; }
        public virtual User User { get; set; }
    }
}
