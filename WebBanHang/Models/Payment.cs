//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebBanHang.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payment
    {
        public Payment()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int PaymentID { get; set; }
        public string PaymentType { get; set; }
    
        public virtual ICollection<Order> Orders { get; set; }
    }
}
