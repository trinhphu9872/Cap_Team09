//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdvisorManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccountUser
    {
        public System.Guid ID { get; set; }
        public string user_code { get; set; }
        public int id_Role { get; set; }
        public string username { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string dateofbirth { get; set; }
        public System.DateTime createtime { get; set; }
        public string picture { get; set; }
    
        public virtual Role Role { get; set; }
    }
}