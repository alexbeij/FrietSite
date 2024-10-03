namespace FrietSite.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
