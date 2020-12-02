namespace NorthwindEFCoreSqlite
{
    public partial class Category
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public byte[] Description { get; set; }
        public byte[] Picture { get; set; }
    }
}
