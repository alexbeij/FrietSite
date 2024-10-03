using Microsoft.AspNetCore.Identity;

namespace FrietSite.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public string? IdentityUserId { get; set; }
        public IdentityUser? IdentityUser { get; set; }
    }
}
