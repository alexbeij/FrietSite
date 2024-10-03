namespace FrietSite.Models
{
    public class Orderviewmodel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public List<int> SelectedProductIds { get; set; } = new List<int>();  
        public string? Description { get; set; }
        public IEnumerable<Product>? AvailableProducts { get; set; }
    }
}
