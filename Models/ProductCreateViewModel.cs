using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrietSite.Models
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
