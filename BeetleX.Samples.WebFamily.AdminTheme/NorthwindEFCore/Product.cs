using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindEFCoreSqlite
{
    [Table("Products")]
    public partial class Product
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public long SupplierID { get; set; }
        public long CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public double? UnitPrice { get; set; }
        public long? UnitsInStock { get; set; }
        public long? UnitsOnOrder { get; set; }
        public long? ReorderLevel { get; set; }
        public long Discontinued { get; set; }
    }
}
