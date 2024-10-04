namespace FrietSite.Models
{
    public class OrderHistory 

    {
        public int Id { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
