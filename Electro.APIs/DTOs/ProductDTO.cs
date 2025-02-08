namespace Electro.APIs.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityInStock { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
    }
}
