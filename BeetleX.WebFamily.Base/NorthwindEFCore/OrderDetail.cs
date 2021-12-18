using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindEFCoreSqlite
{
    [Table("[Order Details]")]
    public partial class OrderDetail
    {
        public long OrderID { get; set; }
        public long ProductID { get; set; }
        public double UnitPrice { get; set; }
        public long Quantity { get; set; }
        public double Discount { get; set; }
    }
}
