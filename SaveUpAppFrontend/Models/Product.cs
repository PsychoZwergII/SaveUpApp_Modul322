// Models/Product.cs
namespace SaveUpAppFrontend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }

}
