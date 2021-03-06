//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication7.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ThekedarOrder
    {
        public int Id { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> StockId { get; set; }
        public Nullable<int> ThekedarId { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public string Quantity { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string Status { get; set; }
        public string IsDelivered { get; set; }
        public Nullable<double> Advance { get; set; }
        public Nullable<double> RemainingBalance { get; set; }
        public Nullable<int> WeekId { get; set; }
    
        public virtual Stock Stock { get; set; }
        public virtual Thekedar Thekedar { get; set; }
        public virtual WeekNumber WeekNumber { get; set; }
    }
}
