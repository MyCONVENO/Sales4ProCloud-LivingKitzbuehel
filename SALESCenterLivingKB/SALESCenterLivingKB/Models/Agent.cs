//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SALESCenterLivingKB.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Agent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agent()
        {
            this.Customer = new HashSet<Customer>();
            this.UserAgents = new HashSet<UserAgents>();
        }
    
        public string AgentID { get; set; }
        public string AgentNumber { get; set; }
        public string AgentConfirmFax { get; set; }
        public string AgentConfirmEmail { get; set; }
        public string AgentRemark { get; set; }
        public decimal AgentCommission { get; set; }
        public string AgentName1 { get; set; }
        public string AgentName2 { get; set; }
        public string AgentCity { get; set; }
        public string AgentZIP { get; set; }
        public string AgentStreet { get; set; }
        public string AgentPhone { get; set; }
        public string AgentFax { get; set; }
        public string AgentEMail { get; set; }
        public System.DateTime SyncDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string AgentMobile { get; set; }
        public string AgentCountryName { get; set; }
        public string AgentCountryCode { get; set; }
        public string AgentClientID { get; set; }
        public Nullable<bool> IsTestData { get; set; }
    
        public virtual Client Client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserAgents> UserAgents { get; set; }
    }
}