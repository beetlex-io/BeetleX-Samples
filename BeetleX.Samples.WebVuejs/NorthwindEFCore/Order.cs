﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindEFCoreSqlite
{
    [Table("orders")]
    public partial class Order
    {
        public long OrderID { get; set; }
        public string CustomerID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public long ShipVia { get; set; }
        public double Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
    }
}
