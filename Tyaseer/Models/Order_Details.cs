//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tyaseer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order_Details
    {
        public int ID { get; set; }
        public double Price { get; set; }
        public int Order_ID { get; set; }
        public int Product_ID { get; set; }
        public int Count { get; set; }
        public bool Accepted { get; set; }
        public System.DateTime DateNeeded { get; set; }
        public Nullable<double> Discount { get; set; }
        public double FinalPrice { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
