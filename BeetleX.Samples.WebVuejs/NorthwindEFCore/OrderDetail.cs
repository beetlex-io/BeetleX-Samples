namespace NorthwindEFCoreSqlite
{
    public partial class OrderDetail
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public double? UnitPrice { get; set; }
        public long? Quantity { get; set; }
        public double? Discount { get; set; }
    }
}
