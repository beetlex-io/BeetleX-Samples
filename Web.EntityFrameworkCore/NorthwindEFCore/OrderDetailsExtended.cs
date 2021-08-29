namespace NorthwindEFCoreSqlite
{
    public partial class OrderDetailsExtended
    {
        public long? OrderId { get; set; }
        public long? ProductId { get; set; }
        public string ProductName { get; set; }
        public double? UnitPrice { get; set; }
        public long? Quantity { get; set; }
        public double? Discount { get; set; }
        public byte[] ExtendedPrice { get; set; }
    }
}
